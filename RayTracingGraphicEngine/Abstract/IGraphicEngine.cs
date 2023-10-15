using Engine3D.Scenes;
using System.Threading.Tasks;

namespace RayTracingGraphicEngine3D.Abstract
{
    public interface IGraphicEngine<SceneType>
        where SceneType : class, IScene
    {
        /// <summary>
        /// Local scene
        /// </summary>
        SceneType LocalScene { get; set; }


        /// <summary>
        /// Render only one frame (without updating)
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
        bool IsUpdating { get; }

        /// <summary>
        /// If true, the screen will be rendered when the scene changes;
        /// Else the screen will be rendered every frame
        /// </summary>
        bool IsOptimalRendering { get; set; }

        /// <summary>
        /// Updates and renders only one frame
        /// </summary>
        void UpdateFrame();

        /// <summary>
        /// Start updating and rendering in real time
        /// </summary>
        void StartUpdating();

        /// <summary>
        /// Stop updating and rendering in real time
        /// </summary>
        Task StopUpdating();
    }
}
