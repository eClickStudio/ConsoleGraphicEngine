using ConsoleGraphicEngine3D.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Light;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using Engine3D.Scenes;
using System.Collections.Generic;

namespace ConsoleGraphicEngine3D.Engine.Basic.Scenes
{
    public interface IRenderableScene<CameraType, RendererType> : IScene
        where CameraType : class, ICamera
        where RendererType : class, IRenderer
    {
        /// <summary>
        /// Main camera for rendering
        /// </summary>
        CameraType MainCamera { get; set; }

        /// <summary>
        /// Main light for rendering
        /// </summary>
        IDirectionLight GlobalLight { get; set; }


        /// <summary>
        /// All renderers
        /// </summary>
        IReadOnlyList<RendererType> Renderers { get; }
    }
}
