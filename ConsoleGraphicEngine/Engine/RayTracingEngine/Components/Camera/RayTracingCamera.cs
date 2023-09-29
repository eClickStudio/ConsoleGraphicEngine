using ConsoleGraphicEngine3D.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine3D.Engine.Basic.Tools;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering;
using Engine3D.Components.Transform;
using System;
using System.Numerics;
using Quaternion = Math3D.Quaternion;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Camera
{
    public class RayTracingCamera : AbstractCamera
    {
        public RayTracingCamera(Vector2Int resolution, Vector2Int charSize, Vector2 cameraAngle, CameraCharSet charSet) 
            : base(resolution, charSize, cameraAngle, charSet)
        {

        }

        /// <summary>
        /// Get ray emitted from camera
        /// </summary>
        /// <param name="screenPosition">Position in screen space</param>
        /// <returns></returns>
        public Ray GetRay(Vector2Int screenPosition)
        {
            if (screenPosition < Vector2Int.Zero || screenPosition >= Resolution)
            {
                throw new ArgumentException($"Screen position is invalid; Min = (0, 0, 0); Max = {Resolution}; " +
                    $"Your argument = {screenPosition}");
            }

            ITransform transform = ParentObject.ThisTransform;

            Vector2 relativeScreenPosition = GetRelativePosition(screenPosition.X, screenPosition.Y);

            Vector2 angularOffset = new Vector2(
                relativeScreenPosition.X * CameraAngle.X / 2,
                relativeScreenPosition.Y * CameraAngle.Y / 2
                );

            //TODO: test it may be mirror effect
            Vector3 rayDirection = Vector3.Normalize(Quaternion.RotateVector(transform.AxisZ, transform.AxisY, angularOffset.X));
            rayDirection = Vector3.Normalize(Quaternion.RotateVector(rayDirection, transform.AxisX, angularOffset.Y));

            if (screenPosition.X == 0 && screenPosition.Y == 0)
            {
                //Console.WriteLine($"cameraAngle = {CameraAngle}");
                //Console.WriteLine();
                //Console.WriteLine($"cameraAxisX = {transform.AxisX}");
                //Console.WriteLine($"cameraAxisY = {transform.AxisY}");
                //Console.WriteLine($"cameraAxisZ = {transform.AxisZ}");
                //Console.WriteLine();
                //Console.WriteLine($"min ray direction = {rayDirection}");
            }

            //if (screenPosition.X == resolution.X - 1 && screenPosition.Y == resolution.Y - 1)
            //{
            //    Console.WriteLine($"cameraAngle = {cameraAngle}");
            //    Console.WriteLine($"max ray direction = {rayDirection}");
            //}

            return new Ray(transform.Position, rayDirection);
        }
    }
}
