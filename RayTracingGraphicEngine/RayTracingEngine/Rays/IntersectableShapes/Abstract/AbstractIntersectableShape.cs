using RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections;
using Engine3D.Components.Transform;
using Engine3D.ChangeTriggers;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes.Abstract
{
    public abstract class AbstractIntersectableShape : AbstractChangebleUpdateble, IIntersectableShape
    {
        protected readonly ITransform transform;

        public AbstractIntersectableShape(in ITransform transform)
        {
            this.transform = transform;
        }

        public abstract ShapeIntersection? GetShapeIntersection(Ray ray);
    }
}
