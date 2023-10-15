using RayTracingGraphicEngine3D.Components.Light.Abstract;
using Engine3D.Scenes;
using System.Collections.Generic;
using RayTracingGraphicEngine3D.Rays;
using RayTracingGraphicEngine3D.Components.Camera.Abstract;

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
        IDirectionLight GlobalLight { get; set; }


        /// <summary>
        /// All intersectables (objects ray can intersect)
        /// </summary>
        IReadOnlyList<IIntersectable> Intersectables { get; }
    }
}
