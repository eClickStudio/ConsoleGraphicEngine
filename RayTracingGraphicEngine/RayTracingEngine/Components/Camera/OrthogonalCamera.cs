using Engine3D.Components.Transform;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using System;
using System.Numerics;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera
{
    public class OrthogonalCamera : AbstractRayTracingCamera
    {
        private Vector2 _cameraSize;
        /// <summary>
        /// The width (along axis X) of camera in units
        /// </summary>
        public Vector2 CameraSize
        {
            get => _cameraSize;
            set
            {
                if (value.X <= 0 || value.Y <= 0 || !value.IsNormal())
                {
                    throw new ArgumentException($"CharsPerUnit is invalid; It should be > 0\n" +
                        $"Value you want to set {value}");
                }

                if (value != _cameraSize)
                {
                    _cameraSize = value;

                    OnChanged();
                }
            }
        }

        public OrthogonalCamera(Vector2Int resolution, Vector2Int charSize, Vector2 cameraSize, CameraCharSet charSet)
            : base(resolution, charSize, charSet)
        {
            CameraSize = cameraSize;
        }

        protected override Ray CreateRay(Vector2 relativePosition)
        {
            ITransform transform = ParentObject.Transform;

            float offsetX = relativePosition.X * CameraSize.X / 2;
            float offsetY = relativePosition.Y * CameraSize.Y / 2;
            Vector3 rayOrigin = transform.Position + transform.AxisX * offsetX + transform.AxisY * offsetY;

            return new Ray(rayOrigin, transform.AxisZ);
        }
    }
}
