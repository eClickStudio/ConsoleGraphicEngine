using ConsoleGraphicEngine.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Basic.Scenes;

namespace ConsoleGraphicEngine.Engine.Abstract
{
    internal interface IGraphicEngine<CameraType, RendererType>
        where CameraType : class, ICamera
        where RendererType : class, IRenderer
    {
        /// <summary>
        /// Rendering scene
        /// </summary>
        IScene<CameraType, RendererType> scene { get; set; }


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
        /// If true, the screen will be rendered when the scene changes;
        /// Else the screen will be rendered every frame
        /// </summary>
        bool isOptimalRendering { get; set; }

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
