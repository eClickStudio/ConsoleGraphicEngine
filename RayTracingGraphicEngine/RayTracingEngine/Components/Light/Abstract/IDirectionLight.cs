using System.Numerics;

namespace RayTracingGraphicEngine3D.Components.Light.Abstract
{
    public interface IDirectionLight : ILight
    {
        /// <summary>
        /// The direction of light
        /// </summary>
        Vector3 Direction { get; set; }
    }
}
