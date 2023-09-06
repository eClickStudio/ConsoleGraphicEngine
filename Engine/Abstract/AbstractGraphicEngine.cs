using ConsoleGraphicEngine.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine.Engine.Basic.Components.Light;
using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Basic.Scenes;
using System.Threading.Tasks;

namespace ConsoleGraphicEngine.Engine.Abstract
{
    internal abstract class AbstractGraphicEngine<CameraType, RendererType> : IGraphicEngine<CameraType, RendererType>
        where CameraType : class, ICamera
        where RendererType : class, IRenderer
    {
        private IScene<CameraType, RendererType> _scene;

        public IScene<CameraType, RendererType> scene 
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
                    camera = value.mainCamera;
                    light = value.globalLight;

                    screen = new char[camera.resolution.X * camera.resolution.Y];
                }
            }
        }

        protected CameraType camera;
        protected IDirectionLight light;

        public bool isRenderingRealTime { get; private set; }

        protected uint _fps; 
        public uint fps 
        { 
            get => _fps;
            set
            {
                _fps = value;
                frameTime = 1000 / value;
            }
        }

        public uint frameTime { get; private set; }

        public uint frameNumber { get; private set; }

        public uint sessionTime { get; private set; }

        public bool isOptimalRendering { get; set; }

        protected char[] screen;

        public AbstractGraphicEngine(uint fps = 3)
        {
            this.fps = fps;
            isOptimalRendering = true;
        }

        private void UpdateFrame()
        {
            frameNumber++;
            sessionTime += frameTime;

            if (scene != null)
            {
                if (scene.Update() || !isOptimalRendering)
                {
                    RenderFrame();
                }
            }
        }

        public abstract void RenderFrame();

        public void StartRenderingRealTime()
        {
            isRenderingRealTime = true;

            Task.Run(() => RenderingRealTime());
        }

        private async void RenderingRealTime()
        {
            while (isRenderingRealTime)
            {
                await Task.Delay((int)frameTime);

                UpdateFrame();
            }
        }

        public void StopRenderingRealTime()
        {
            isRenderingRealTime = false;
        }
    }
}
