using System.Numerics;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract
{
    public interface IDirectionLight : ILight
    {
        /// <summary>
        /// The direction of light in local space
        /// </summary>
        Vector3 LocalDirection { get; set; }

        //TODO: realise worldDirection.set

        /// <summary>
        /// The direction of light in world space
        /// </summary>
        Vector3 WorldDirection { get; }
    }
}
