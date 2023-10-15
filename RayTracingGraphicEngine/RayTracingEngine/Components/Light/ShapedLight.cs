using RayTracingGraphicEngine3D.Components.Light.Abstract;
using RayTracingGraphicEngine3D.Rays;

namespace RayTracingGraphicEngine3D.Components.Light
{
    public class ShapedLight : AbstractLight, IIntersectable
    {
        public IIntersectableShape Shape { get; private set; }

        public ShapedLight(in IIntersectableShape lightShape)
        {
            Shape = lightShape;
        }
    }
}
