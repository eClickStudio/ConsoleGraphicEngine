using RayTracingGraphicEngine3D.Components.Light.Abstract;
using System;
using System.Numerics;

namespace RayTracingGraphicEngine3D.Components.Light
{
    public class DirectionLight : AbstractLight, IDirectionLight
    {
        //TODO: light direction must be depended on transform rotation

        private Vector3 _direction;
        public Vector3 Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (value == Vector3.Zero)
                {
                    throw new ArgumentException($"Direction of light is invalid; " +
                        $"Value can not be = (0, 0, 0); Value you want to set {value}");
                }

                if (_direction != value)
                {
                    _direction = Vector3.Normalize(value);

                    OnChanged();
                }
            }
        }

        public DirectionLight(Vector3 direction, float intensity)
        {
            Direction = direction;
            Intensity = intensity;
        }
    }
}
