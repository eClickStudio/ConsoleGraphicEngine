using ConsoleGraphicEngine3D.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Light;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine3D.Engine.Basic.Scenes;
using System;
using System.Threading.Tasks;

namespace ConsoleGraphicEngine3D.Engine.Abstract
{
    public abstract class AbstractGraphicEngine<CameraType, RendererType> : IGraphicEngine<CameraType, RendererType>
        where CameraType : class, ICamera
        where RendererType : class, IRenderer
    {
        private IRenderableScene<CameraType, RendererType> _scene;

        public IRenderableScene<CameraType, RendererType> RenderingScene 
        { 
            get
            {
                return _scene;
            }
            set
            {
                _scene = value;

                if (value != null)
                {
                    camera = value.MainCamera;
                    light = value.GlobalLight;

                    screen = new char[camera.Resolution.X * camera.Resolution.Y];
                }
            }
        }

        protected CameraType camera;
        protected IDirectionLight light;

        private Task _renderingRealTimeTask;

        public bool IsRenderingRealTime { get; private set; }

        protected uint _fps; 
        public uint FPS 
        { 
            get => _fps;
            set
            {
                _fps = value;
                FrameTime = 1000 / value;
            }
        }

        public uint FrameTime { get; private set; }

        public uint FrameNumber { get; private set; }

        public uint SessionTime { get; private set; }

        public bool IsOptimalRendering { get; set; }

        protected char[] screen;

        public AbstractGraphicEngine(uint FPS = 3)
        {
            this.FPS = FPS;
            IsOptimalRendering = true;
        }

        private void UpdateFrame()
        {
            FrameNumber++;
            SessionTime += FrameTime;

            if (RenderingScene != null)
            {
                if (RenderingScene.Update(FrameTime) || !IsOptimalRendering)
                {
                    RenderFrame();
                }
            }
        }

        public abstract void RenderFrame();

        public async void StartRenderingRealTime()
        {
            IsRenderingRealTime = true;

            _renderingRealTimeTask = RenderingRealTime();

            await _renderingRealTimeTask;
        }

        private async Task RenderingRealTime()
        {
            while (IsRenderingRealTime)
            {
                await Task.Delay((int)FrameTime);

                UpdateFrame();
            }
        }

        public async Task StopRenderingRealTime()
        {
            IsRenderingRealTime = false;

            if (_renderingRealTimeTask != null)
            {
                await _renderingRealTimeTask;
            }

            Console.WriteLine("Rendering stopped. Type 'start' to start rendering again");
        }
    }
}
