using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using Engine3D.Components.Abstract;
using System.Numerics;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract
{
    public interface ICamera : IComponent
    {
        /// <summary>
        /// Screen resolution in chars
        /// </summary>
        Vector2Int Resolution { get; }

        /// <summary>
        /// Camera addmissible chars
        /// </summary>
        CameraCharSet CharSet { get; set; }

        /// <summary>
        /// Get char using brightness
        /// </summary>
        /// <param name="intensity">float between 0 and 1. Can be null if there is no color</param>
        /// <returns>Char</returns>
        char GetChar(float? intensity);
    }
}
