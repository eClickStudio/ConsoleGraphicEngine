using Engine3D.Components.Abstract;

namespace RayTracingGraphicEngine3D.Components.Light.Abstract
{
    public interface ILight : IComponent
    {
        /// <summary>
        /// The intensivety of light; Must be >= 0
        /// </summary>
        float Intensity { get; set; }
    }
}
