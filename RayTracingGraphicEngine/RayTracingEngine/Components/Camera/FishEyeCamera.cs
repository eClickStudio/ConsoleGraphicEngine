using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using Engine3D.Components.Transform;
using System;
using System.Numerics;
using Quaternion = MathExtensions.Quaternion;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera
{
    public class FishEyeCamera : AbstractRayTracingCamera
    {
        private Vector2 MIN_CAMERA_ANGLE { get; } = Vector2.Zero;
        private Vector2 MAX_CAMERA_ANGLE { get; } = Vector2.One * (float)(Math.PI * 2);

        private Vector2 _cameraAngle;
        public Vector2 CameraAngle
        {
            get
            {
                return _cameraAngle;
            }
            set
            {
                if (!value.IsNormal() || value != Vector2.Clamp(value, MIN_CAMERA_ANGLE, MAX_CAMERA_ANGLE))
                {
                    throw new ArgumentException($"Camera angle is invalid; " +
                        $"Min = {MIN_CAMERA_ANGLE}; Max = {MAX_CAMERA_ANGLE}; Value you want to set {value}");
                }

                if (_cameraAngle != value)
                {
                    _cameraAngle = value;

                    OnChanged();
                }
            }
        }

        public FishEyeCamera(Vector2Int resolution, Vector2Int charSize, Vector2 cameraAngle, CameraCharSet charSet) 
            : base(resolution, charSize, charSet)
        {
            if (cameraAngle != Vector2.Clamp(cameraAngle, MIN_CAMERA_ANGLE, MAX_CAMERA_ANGLE))
            {
                throw new ArgumentException($"Camera angle is invalid; cameraAngle = {cameraAngle};" +
                    $"It must be > {MIN_CAMERA_ANGLE} and < {MAX_CAMERA_ANGLE}");
            }

            CameraAngle = cameraAngle;
        }

        protected override Ray CreateRay(Vector2 relativePosition)
        {
            ITransform transform = ParentObject.Transform;

            Vector2 angularOffset = new Vector2(
                relativePosition.X * CameraAngle.X / 2,
                relativePosition.Y * CameraAngle.Y / 2
                );

            //TODO: test it may be mirror effect
            Vector3 rayDirection = Vector3.Normalize(Quaternion.RotateVector(transform.AxisZ, transform.AxisY, angularOffset.X));
            rayDirection = Vector3.Normalize(Quaternion.RotateVector(rayDirection, transform.AxisX, angularOffset.Y));

            return new Ray(transform.Position, rayDirection);
        }
    }
}
