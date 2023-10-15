using RayTracingGraphicEngine3D.Rays.Intersections;
using Engine3D.Components.Transform;
using Engine3D.ChangeTriggers;

namespace RayTracingGraphicEngine3D.Rays.IntersectableShapes.Abstract
{
    public abstract class AbstractIntersectableShape : AbstractChangebleUpdateble, IIntersectableShape
    {
        //TODO: maybe its redundant
        protected const float MIN_RAY_STEP = 0;

        protected readonly ITransform transform;

        public AbstractIntersectableShape(in ITransform transform)
        {
            this.transform = transform;
        }

        public abstract ShapeIntersection? GetShapeIntersection(Ray ray);
    }
}
