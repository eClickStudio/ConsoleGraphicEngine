using System.Numerics;
using Engine3D.Components.Transform;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes
{
    public class CubeShape : BoxShape
    {
        public CubeShape(in ITransform transform, float edgeLenght) : base(transform, Vector3.One * edgeLenght) { }
    }
}
