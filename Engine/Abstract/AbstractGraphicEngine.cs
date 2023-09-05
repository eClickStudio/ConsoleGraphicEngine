using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Basic.Scenes;
using System.Threading.Tasks;

namespace ConsoleGraphicEngine.Engine.Abstract
{
    internal abstract class AbstractGraphicEngine<RendererType> : IGraphicEngine<RendererType>
        where RendererType : class, IRenderer
    {
        public IScene<RendererType> scene { get; set; }

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


        public AbstractGraphicEngine(uint fps = 3)
        {
            this.fps = fps;
        }

        private void UpdateFrame()
        {
            frameNumber++;
            sessionTime += frameTime;

            bool didChange = scene.Update();

            if (didChange)
            {
                RenderFrame();
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
