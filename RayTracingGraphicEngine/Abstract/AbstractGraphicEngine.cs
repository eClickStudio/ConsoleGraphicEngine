using RayTracingGraphicEngine3D.Abstract.Scenes;
using RayTracingGraphicEngine3D.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.Components.Light.Abstract;
using RayTracingGraphicEngine3D.Scenes;
using System;
using System.Threading.Tasks;

namespace RayTracingGraphicEngine3D.Abstract
{
    public abstract class AbstractGraphicEngine<SceneType, CameraType> : IGraphicEngine<SceneType>
        where SceneType : class, IRenderableScene<CameraType>
        where CameraType : class, ICamera
    {
        private SceneType _scene;

        public SceneType LocalScene 
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

        private Task _updatingTask;

        public bool IsUpdating { get; private set; }

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

        public void UpdateFrame()
        {
            FrameNumber++;
            SessionTime += FrameTime;

            if (LocalScene != null)
            {
                if (LocalScene.Update(FrameTime) || !IsOptimalRendering)
                {
                    RenderFrame();
                }
            }
        }

        public abstract void RenderFrame();

        public async void StartUpdating()
        {
            if (!IsUpdating)
            {
                Console.WriteLine("Updating started");

                IsUpdating = true;
                _updatingTask = Updating();

                await _updatingTask;
            }
        }

        private async Task Updating()
        {
            while (IsUpdating)
            {
                await Task.Delay((int)FrameTime);

                UpdateFrame();
            }
        }

        public async Task StopUpdating()
        {
            if (IsUpdating)
            {
                IsUpdating = false;

                if (_updatingTask != null)
                {
                    await _updatingTask;
                }

                Console.WriteLine("Updating stopped");
            }
        }
    }
}
