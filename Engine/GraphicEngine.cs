using ConsoleGraphicEngine.Engine.Objects.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.Light;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Scenes;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace ConsoleGraphicEngine.Engine
{
    internal class GraphicEngine
    {
        private const int _FPS = 1;
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


        public GraphicEngine(in IScene scene, int rayIterations)
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

        private float RenderRay(Ray ray)
        {
            //float brightness = _light.intensivety;

            //for (int i = 0; i < _rayIterations; i++)
            //{
            //    Intersection? nearestIntersection = GetNearestIntersection(ray);
            //    if (nearestIntersection.HasValue)
            //    {
            //        Vector3 position = ray.startPosition + ray.direction * nearestIntersection.Value.intersectionDistance;
            //        Ray normal = nearestIntersection.Value.intersectedRenderer.GetNormal(position);

            //        ray = Ray.Reflect(ray, normal);

            //        brightness *= (1 - Vector3.Dot(Vector3.Normalize(nearestIntersection.Value), _light.direction)) * _light.intensivety;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            //return brightness;

            Intersection? nearestIntersection = GetNearestIntersection(ray);
            if (nearestIntersection.HasValue)
            {
                Vector3 position = ray.startPosition + ray.direction * nearestIntersection.Value.intersectionDistance;
                Ray normal = nearestIntersection.Value.intersectedRenderer.GetNormal(position);

                ray = Ray.Reflect(ray, normal);

                return (1 - Vector3.Dot(normal.direction, _light.direction)) * _light.intensivety;
            }

            return 0;
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

                    foreach (float intersectionDistance in distances)
                    {
                        if (intersectionDistance < minIntersectionDistance)
                        {
                            minIntersectionDistance = intersectionDistance;

                            intersectedRenderer = visibleObject.renderer;
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
