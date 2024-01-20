using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using Engine3D.Components.Abstract;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering.Abstract
{
    public interface IShapeRenderer : IComponent, IRenderer, IIntersectable
    {
        /// <summary>
        /// Get the ray reflected from the surface at the point of intersection of the initial ray with this render
        /// </summary>
        /// <param name="lightRay">Ray</param>
        /// <param name="normalRay">Normal to the surface at the intersection point</param>
        /// <returns></returns>
        LightRay GetReflectedRay(LightRay lightRay, Ray normalRay);

        /// <summary>
        /// get the ray refracted from the surface at the point of intersection of the initial ray with this render
        /// </summary>
        /// <param name="lightRay">Ray</param>
        /// <param name="normalRay">Normal to the surface at the intersection point</param>
        /// <returns></returns>
        LightRay GetRefractedRay(LightRay lightRay, Ray normalRay);
    }
}
