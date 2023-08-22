using ConsoleGraphicEngine.Engine.Objects;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
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

        private bool _isRendering;

        private uint _frameNumber;
        private uint _deltaTime;
        private uint _time;

        private IScene _scene { get; }
        private Camera _camera { get; }
        private Vector2Int _resolution { get; }
        private char[] _screen;


        public GraphicEngine(in IScene scene)
        {
            _scene = scene;
            _camera = scene.mainCamera;

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

        private void RenderFrame()
        {
            for (int x = 0; x < _resolution.X; x++)
            {
                for (int y = 0; y < _resolution.Y; y++)
                {
                    Vector2Int screenPosition = new Vector2Int(x, y);

                    char pixel = _camera.GetPixel(RenderRay(screenPosition));

                    _screen[x + y * _resolution.X] = pixel;
                }
            }

            Console.Write(_screen);
        }

        private float RenderRay(Vector2Int screenPosition)
        {
            Vector3 rayDirection = _camera.GetRayDirection(screenPosition);
            Console.WriteLine(rayDirection);

            foreach (IObject3D object3d in _scene.GetObjects())
            {
                VisibleObject visibleObject = object3d as VisibleObject;

                if (visibleObject != null)
                {
                    Ray ray = new Ray(_camera.parentObject.transform.position, rayDirection);
                    IReadOnlyList<Vector3> intersecions = visibleObject.renderer.Intersect(ray);
                    
                    if (intersecions != null && intersecions.Count > 0)
                    {
                        Console.WriteLine("Intersect!");
                        return 1;
                    }
                }
            }

            //screenPosition.X += (float)Math.Sin(_time * 0.005f);

            //float r = (float)Math.Sqrt(screenPosition.X * screenPosition.X + screenPosition.Y * screenPosition.Y);

            //if (r < 0.5f)
            //{
            //    return 1 - r / 0.5f;
            //}

            return 0;
        }
    }
}
