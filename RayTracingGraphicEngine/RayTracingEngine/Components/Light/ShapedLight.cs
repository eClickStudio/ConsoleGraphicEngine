using RayTracingGraphicEngine3D.Components.Light.Abstract;
using RayTracingGraphicEngine3D.Rays;

namespace RayTracingGraphicEngine3D.Components.Light
{
    public class ShapedLight : AbstractLight, IIntersectable
    {
        /// <summary>
        /// Shape of this line
        /// </summary>
        public IIntersectableShape Shape { get; set; }

        public ShapedLight(in IIntersectableShape lightShape)
        {
            Shape = lightShape;
        }
    }
}
