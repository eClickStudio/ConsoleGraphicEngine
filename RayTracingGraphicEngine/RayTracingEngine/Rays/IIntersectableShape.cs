using RayTracingGraphicEngine3D.Rays.Intersections;

namespace RayTracingGraphicEngine3D.Rays
{
    public interface IIntersectableShape
    {
        /// <summary>
        /// Get shape intersection data
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        ShapeIntersection? GetShapeIntersection(Ray ray);
    }
}
