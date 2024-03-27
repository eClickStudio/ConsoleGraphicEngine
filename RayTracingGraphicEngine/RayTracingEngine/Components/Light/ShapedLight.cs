using RayTracingGraphicEngine3D.RayTracingEngine.Rays;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Light
{
    public class ShapedLight : Light, IIntersectable
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
