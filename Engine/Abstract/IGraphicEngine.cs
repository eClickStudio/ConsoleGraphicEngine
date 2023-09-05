using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Basic.Scenes;

namespace ConsoleGraphicEngine.Engine.Abstract
{
    internal interface IGraphicEngine<RendererType>
        where RendererType : class, IRenderer
    {
        /// <summary>
        /// Rendering scene
        /// </summary>
        IScene<RendererType> scene { get; set; }


        /// <summary>
        /// Render only one frame
        /// </summary>
        void RenderFrame();


        /// <summary>
        /// frame per second
        /// </summary>
        uint fps { get; set; }

        /// <summary>
        /// Time of one frame in milliseconds
        /// </summary>
        uint frameTime { get; }

        /// <summary>
        /// The number of the last frame
        /// </summary>
        uint frameNumber { get; }

        /// <summary>
        /// Time when this session is rendering in real time. Time in milliseconds       
        /// </summary>
        uint sessionTime { get; }


        /// <summary>
        /// If real time rendering in progress?
        /// </summary>
        bool isRenderingRealTime { get; }

        /// <summary>
        /// Start rendering in real time
        /// </summary>
        void StartRenderingRealTime();

        /// <summary>
        /// Stop rendering in real time
        /// </summary>
        void StopRenderingRealTime();
    }
}
