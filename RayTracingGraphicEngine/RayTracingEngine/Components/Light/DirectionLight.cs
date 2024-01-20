using Engine3D.Components.Transform;
using Engine3D.Objects;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;
using System;
using System.Numerics;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Light
{
    public class DirectionLight : AbstractLight, IDirectionLight
    {
        //TODO: light direction must be depended on transform rotation

        private Vector3 _localDirection;
        public Vector3 LocalDirection
        {
            get => _localDirection;
            set 
            {
                if (_localDirection != value)
                {
                    Vector3 direction = Vector3.Normalize(value);

                    if (!direction.IsNormal())
                    {
                        throw new ArgumentException($"Direction of light is invalid;\n" +
                            $"Value you want to set {direction}");
                    }

                    _localDirection = direction;

                    OnChanged();
                }
            }
        }

        public Vector3 WorldDirection { get; private set; }

        public DirectionLight(Vector3 direction, float intensity) : base(intensity)
        {
            if (direction == Vector3.Zero || !direction.IsNormal())
            {
                throw new ArgumentException($"Direction of light is invalid;\n" +
                    $"Value you want to set {direction}");
            }

            _localDirection = direction;
            WorldDirection = direction;

            OnAttachedToObjectEvent += OnParentObjectSet;
        }

        private void OnParentObjectSet(IObject3D parentObject)
        {
            parentObject.Transform.OnRotationChangedEvent += UpdateLightDirection;
        }

        private void UpdateLightDirection()
        {
            WorldDirection = ParentObject.Transform.ConvertVectorFromLocalToWorld(LocalDirection);
        }
    }
}
