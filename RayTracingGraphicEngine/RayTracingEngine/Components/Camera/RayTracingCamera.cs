using RayTracingGraphicEngine3D.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.Tools;
using Engine3D.Components.Transform;
using System;
using System.Numerics;
using Quaternion = MathExtensions.Quaternion;
using RayTracingGraphicEngine3D.Components.Rendering;

namespace RayTracingGraphicEngine3D.Components.Camera
{
    public class RayTracingCamera : AbstractCamera, IRayTracingCamera
    {
        public RayTracingCamera(Vector2Int resolution, Vector2Int charSize, Vector2 cameraAngle, CameraCharSet charSet) 
            : base(resolution, charSize, cameraAngle, charSet)
        {

        }

        public LightRay GetRay(Vector2Int screenPosition)
        {
            if (screenPosition < Vector2Int.Zero || screenPosition >= Resolution)
            {
                throw new ArgumentException($"Screen position is invalid; Min = (0, 0, 0); Max = {Resolution}; " +
                    $"Your argument = {screenPosition}");
            }

            ITransform transform = ParentObject.Transform;

            Vector2 relativeScreenPosition = GetRelativePosition(screenPosition.X, screenPosition.Y);

            Vector2 angularOffset = new Vector2(
                relativeScreenPosition.X * CameraAngle.X / 2,
                relativeScreenPosition.Y * CameraAngle.Y / 2
                );

            //TODO: test it may be mirror effect
            Vector3 rayDirection = Vector3.Normalize(Quaternion.RotateVector(transform.AxisZ, transform.AxisY, angularOffset.X));
            rayDirection = Vector3.Normalize(Quaternion.RotateVector(rayDirection, transform.AxisX, angularOffset.Y));

            return new LightRay(transform.Position, rayDirection, 1);
        }

        public Material GetEnvironmentMaterial()
        {
            //TODO: return real Environment material

            return Material.Vacuum;
        }
    }
}
