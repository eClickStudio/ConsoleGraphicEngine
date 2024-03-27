using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;
using Engine3D.Scenes;
using System.Collections.Generic;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract;

namespace RayTracingGraphicEngine3D.Abstract.Scenes
{
    public interface IRenderableScene<CameraType> : IScene
        where CameraType : ICamera
    {
        /// <summary>
        /// Main camera for rendering
        /// </summary>
        CameraType MainCamera { get; set; }

        /// <summary>
        /// Main light for rendering
        /// </summary>
        ILight GlobalLight { get; set; }

        /// <summary>
        /// Direction lights of this scene
        /// </summary>
        IList<IDirectionLight> DirectionLights { get; }

        /// <summary>
        /// All intersectables (objects ray can intersect)
        /// </summary>
        IReadOnlyList<IIntersectable> Intersectables { get; }
    }
}
