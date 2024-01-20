using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Light
{
    public class ShapedLight : AbstractLight, IIntersectable
    {
        /// <summary>
        /// Shape of this line
        /// </summary>
        public IIntersectableShape Shape { get; set; }

        public ShapedLight(in IIntersectableShape lightShape, float intensity) : base(intensity)
        {
            Shape = lightShape;
        }
    }
}
