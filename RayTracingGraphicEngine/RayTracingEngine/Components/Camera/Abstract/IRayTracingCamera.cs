using RayTracingGraphicEngine3D.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.Components.Rendering;
using RayTracingGraphicEngine3D.Tools;

namespace RayTracingGraphicEngine3D.Components.Camera.Abstract
{
    public interface IRayTracingCamera : ICamera
    {
        /// <summary>
        /// Get ray emitted from camera
        /// </summary>
        /// <param name="screenPosition">Position in screen space</param>
        /// <returns></returns>
        LightRay GetRay(Vector2Int screenPosition);

        /// <summary>
        /// The material of environment around the camera
        /// </summary>
        /// <returns></returns>
        Material GetEnvironmentMaterial();
    }
}
