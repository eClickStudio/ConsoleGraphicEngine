using ConsoleGraphicEngine3D.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine3D.Engine.Basic.Scenes;
using System.Threading.Tasks;

namespace ConsoleGraphicEngine3D.Engine.Abstract
{
    public interface IGraphicEngine<CameraType, RendererType>
        where CameraType : class, ICamera
        where RendererType : class, IRenderer
    {
        /// <summary>
        /// Rendering scene
        /// </summary>
        IRenderableScene<CameraType, RendererType> RenderingScene { get; set; }


        /// <summary>
        /// Render only one frame
        /// </summary>
        void RenderFrame();


        /// <summary>
        /// frame per second
        /// </summary>
        uint FPS { get; set; }

        /// <summary>
        /// Time of one frame in milliseconds
        /// </summary>
        uint FrameTime { get; }

        /// <summary>
        /// The number of the last frame
        /// </summary>
        uint FrameNumber { get; }

        /// <summary>
        /// Time when this session is rendering in real time. Time in milliseconds       
        /// </summary>
        uint SessionTime { get; }


        /// <summary>
        /// If real time rendering in progress?
        /// </summary>
        bool IsRenderingRealTime { get; }

        /// <summary>
        /// If true, the screen will be rendered when the scene changes;
        /// Else the screen will be rendered every frame
        /// </summary>
        bool IsOptimalRendering { get; set; }

        /// <summary>
        /// Start rendering in real time
        /// </summary>
        void StartRenderingRealTime();

        /// <summary>
        /// Stop rendering in real time
        /// </summary>
        Task StopRenderingRealTime();
    }
}
