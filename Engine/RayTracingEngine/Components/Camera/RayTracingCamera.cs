using ConsoleGraphicEngine.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine.Engine.Basic.Components.Transform;
using ConsoleGraphicEngine.Engine.Basic.Tools;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering;
using System;
using System.Numerics;
using Quaternion = ConsoleGraphicEngine.Engine.Basic.Tools.Quaternion;

namespace ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Camera
{
    class RayTracingCamera : AbstractCamera
    {
        public RayTracingCamera(Vector2Int resolution, Vector2Int charSize, Vector2 cameraAngle, float charsPerUnit, CameraCharSet charSet) 
            : base(resolution, charSize, cameraAngle, charsPerUnit, charSet)
        {

        }

        /// <summary>
        /// Get ray emitted from camera
        /// </summary>
        /// <param name="screenPosition">Position in screen space</param>
        /// <returns></returns>
        public Ray GetRay(Vector2Int screenPosition)
        {
            if (screenPosition < Vector2Int.Zero || screenPosition >= resolution)
            {
                throw new ArgumentException($"Screen position is invalid; Min = (0, 0, 0); Max = {resolution}; " +
                    $"Your argument = {screenPosition}");
            }

            ITransform transform = parentObject.transform;

            Vector2 relativeScreenPosition = GetRelativePosition(screenPosition.X, screenPosition.Y);

            Vector2 angularOffset = new Vector2(
                relativeScreenPosition.X * cameraAngle.X / 2,
                relativeScreenPosition.Y * cameraAngle.Y / 2
                );

            //TODO: test it may be mirror effect
            Vector3 rayDirection = Vector3.Normalize(Quaternion.RotateVector(transform.axisZ, transform.axisY, angularOffset.X));
            rayDirection = Vector3.Normalize(Quaternion.RotateVector(rayDirection, transform.axisX, angularOffset.Y));

            return new Ray(transform.position, rayDirection);
        }
    }
}
