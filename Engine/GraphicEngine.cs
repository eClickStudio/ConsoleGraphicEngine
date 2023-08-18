using ConsoleGraphicEngine.Engine.ConsoleSetter;
using ConsoleGraphicEngine.Engine.Objects;
using ConsoleGraphicEngine.Engine.Objects.Components;
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
        private const int _FPS = 10;
        private const int _FRAME_TIME = 1000 / _FPS;

        private bool _isRendering;

        private uint _frameNumber;
        private uint _deltaTime;
        private uint _time;

        public IScene scene;

        public GraphicEngine(in Scene scene)
        {
            this.scene = scene;
        }

        public void StartRendering()
        {
            _isRendering = true;

            Camera camera = scene.mainCamera;
            Vector2Int resolution = camera.resolution;

            int screenCapacity = resolution.X * resolution.Y;

            char[] screen = new char[screenCapacity];

            while (_isRendering)
            {
                for (int x = 0; x < resolution.X; x++)
                {
                    for (int y = 0; y < resolution.Y; y++)
                    {
                        Vector2 position = camera.GetRelativePosition(x, y);

                        char pixel = camera.GetPixel(RenderFrame(position));

                        screen[x + y * resolution.X] = pixel;
                    }
                }

                Thread.Sleep(_FRAME_TIME);

                _frameNumber++;
                _deltaTime = _FRAME_TIME;
                _time += _deltaTime;

                //screen[(_consoleSize.x) * (_consoleSize.y) - 1] = '@';
                Console.Write(screen);
            }
        }

        public void StopRendering()
        {
            _isRendering = false;
        }

        private float RenderFrame(Vector2 position)
        {
            position.X += (float)Math.Sin(_time * 0.005f);

            float r = (float)Math.Sqrt(position.X * position.X + position.Y * position.Y);

            if (r < 0.5f)
            {
                return 1 - r / 0.5f;
            }

            return 0;
        }
    }
}
