using ConsoleGraphicEngine.Engine.Objects.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.Light;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Scenes;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Quaternion = ConsoleGraphicEngine.Engine.Tools.Quaternion;

namespace ConsoleGraphicEngine.Engine
{
    internal class RayTracingGraphicEngine
    {
        private const int _FPS = 3;
        private const int _FRAME_TIME = 1000 / _FPS;

        private int _rayIterations { get; }

        private bool _isRendering;

        private uint _frameNumber;
        private uint _deltaTime;
        private uint _time;

        private IScene _scene { get; }
        private Camera _camera { get; }
        private GlobalLight _light { get; }
        private Vector2Int _resolution { get; }
        private char[] _screen;


        public RayTracingGraphicEngine(in IScene scene, int rayIterations)
        {
            _scene = scene;
            _camera = scene.mainCamera;
            _light = scene.globalLight;

            _rayIterations = rayIterations;

            _resolution = _camera.resolution;
            _screen = new char[_resolution.X * _resolution.Y];
        }

        public void StartRendering()
        {
            _isRendering = true;

            while (_isRendering)
            {
                Thread.Sleep(_FRAME_TIME);

                _frameNumber++;
                _deltaTime = _FRAME_TIME;
                _time += _deltaTime;

                RenderFrame();

                _light.direction = Quaternion.RotateVector(_light.direction, new Vector3(0, 1, 0), (float)_deltaTime / 1000 * (float)Math.PI / 10);
            }
        }

        public void StopRendering()
        {
            _isRendering = false;
        }

        public void RenderFrame()
        {
            for (int x = 0; x < _resolution.X; x++)
            {
                for (int y = 0; y < _resolution.Y; y++)
                {
                    Vector2Int screenPosition = new Vector2Int(x, y);

                    Ray ray = new Ray(
                        _camera.parentObject.transform.position, 
                        _camera.GetRayDirection(screenPosition)
                    );

                    char pixel = _camera.GetPixel(RenderRay(ray));

                    _screen[x + y * _resolution.X] = pixel;
                }
            }

            Console.Write(_screen);
        }

        private float? RenderRay(Ray ray)
        {
            float? brightness = _light.intensivety;

            bool isIntersect = false;

            for (int i = 0; i < _rayIterations; i++)
            {
                Intersection? nearestIntersection = GetNearestIntersection(ray);
                if (nearestIntersection.HasValue)
                {
                    isIntersect = true;

                    IObjectRenderer renderer = nearestIntersection.Value.intersectedRenderer;

                    Ray? normal = renderer.GetNormal(ray);

                    if (normal.HasValue)
                    {
                        ray = Ray.Reflect(ray, normal.Value);

                        brightness *= renderer.GetBrightness(normal.Value.direction, _light.direction);
                    }
                }
                else
                {
                    if (!isIntersect)
                    {
                        brightness = null;
                    }

                    break;
                }
            }

            return brightness;
        }

        private Intersection? GetNearestIntersection(Ray ray)
        {
            bool isIntersect = false;

            float minIntersectionDistance = float.MaxValue;
            IObjectRenderer intersectedRenderer = null;

            foreach (IVisibleObject visibleObject in _scene.GetVisibleObjects())
            {
                IReadOnlyList<float> distances = visibleObject.renderer.GetIntersectionDistances(ray);

                if (distances != null && distances.Count > 0)
                {
                    isIntersect = true;
                    intersectedRenderer = visibleObject.renderer;

                    foreach (float intersectionDistance in distances)
                    {
                        if (intersectionDistance < 0)
                        {
                            throw new Exception($"intersection distance cannot be less than 0; intersectionDistance = {intersectionDistance}");
                        }

                        if (intersectionDistance < minIntersectionDistance)
                        {
                            minIntersectionDistance = intersectionDistance;
                        }
                    }
                }
            }

            if (isIntersect)
            {
                return new Intersection(intersectedRenderer, minIntersectionDistance);
            }

            return null;
        }
    }
}
