using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Tools;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Rendering
{
    internal interface ICamera : IComponent
    {
        /// <summary>
        /// Screen resolution in chars
        /// </summary>
        Vector2Int resolution { get; }

        /// <summary>
        /// How much chars must be in one unit?
        /// </summary>
        public float charsPerUnit { get; set; }

        /// <summary>
        /// Size of screen in world space; Size of screen in units
        /// </summary>
        Vector2 screenWorldSize { get; }

        /// <summary>
        /// Angular space in Radians visible to the camera. Max 2*PI
        /// </summary>
        Vector2 cameraAngle { get; set; }

        /// <summary>
        /// Camera addmissible chars
        /// </summary>
        CameraCharSet charSet { get; set; }

        /// <summary>
        /// Get char using brightness
        /// </summary>
        /// <param name="brightness">float between 0 and 1. Can be null if there is no color</param>
        /// <returns>Char</returns>
        char GetChar(float? brightness);

        /// <summary>
        /// Get positon of screen point relative center of screen; min X, Y = -1; max X, Y = 1 
        /// </summary>
        /// <param name="absoluteX">Screen point X</param>
        /// <param name="absoluteY">Screen point Y</param>
        /// <returns>Relative position</returns>
        Vector2 GetRelativePosition(int absoluteX, int absoluteY);
    }
}
