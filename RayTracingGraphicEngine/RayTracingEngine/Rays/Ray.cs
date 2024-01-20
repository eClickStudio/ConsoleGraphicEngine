using MathExtensions;
using System;
using System.Numerics;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays
{
    public struct Ray
    {
        private Vector3 _origin;

        /// <summary>
        /// Ray start point
        /// </summary>
        public Vector3 Origin
        {
            get => _origin;
            set
            {
                if (!value.IsNormal())
                {
                    throw new ArgumentException("Origin of ray cannot be NaN!");
                }

                _origin = value;
            }
        }


        private Vector3 _direction;

        /// <summary>
        /// Direction of ray (normalized)
        /// </summary>
        public Vector3 Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (value == Vector3.Zero || !value.IsNormal())
                {
                    throw new ArgumentException("Direction of ray cannot be NaN and zero!");
                }

                _direction = Vector3.Normalize(value);
            }
        }

        public Ray(Vector3 direction)
        {
            _origin = Vector3.Zero;
            _direction = Vector3.Zero;

            Direction = direction;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            _origin = origin;
            _direction = direction;

            Origin = origin;
            Direction = direction;
        }

        public static bool operator ==(Ray ray1, Ray ray2)
        {
            return ray1.Origin == ray2.Origin && ray1.Direction == ray2.Direction;
        }

        public static bool operator !=(Ray ray1, Ray ray2)
        {
            return !(ray1 == ray2);
        }

        public static Ray Reflect(Ray ray, Ray normal)
        {
            return new Ray(normal.Origin, Vector3.Reflect(ray.Direction, normal.Direction));
        }

        public override string ToString()
        {
            return $"{Origin}; {Direction}";
        }

        public override bool Equals(object obj)
        {
            return obj is Ray ray &&
                   Origin.Equals(ray.Origin) &&
                   _direction.Equals(ray._direction);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Origin, _direction);
        }
    }
}
