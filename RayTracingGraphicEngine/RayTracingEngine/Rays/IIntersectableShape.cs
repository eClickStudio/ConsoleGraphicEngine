using Engine3D.ChangeTriggers;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays
{
    public interface IIntersectableShape : IChangebleUpdateble
    {
        /// <summary>
        /// Get shape intersection data
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        ShapeIntersection? GetShapeIntersection(Ray ray);
    }
}
