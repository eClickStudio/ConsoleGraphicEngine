using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract
{
    public interface IRayTracingCamera : ICamera
    {
        /// <summary>
        /// Material outside the camera
        /// </summary>
        Material? SceneEnvironmentMaterial { get; set; }

        /// <summary>
        /// Get ray emitted from camera
        /// </summary>
        /// <param name="screenPosition">Position in screen space</param>
        /// <returns></returns>
        LightRay GetRay(Vector2Int screenPosition);
    }
}
