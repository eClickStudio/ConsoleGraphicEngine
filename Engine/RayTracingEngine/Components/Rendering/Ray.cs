using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering
{
    internal struct Ray
    {
        public Vector3 origin;

        private Vector3 _direction;
        public Vector3 direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (value == Vector3.Zero)
                {
                    throw new ArgumentException("Direction of ray cannot be zero!");
                }

                _direction = Vector3.Normalize(value);
            }
        }

        public Ray(Vector3 direction)
        {
            origin = new Vector3();

            _direction = new Vector3();
            this.direction = direction;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;

            _direction = new Vector3();
            this.direction = direction;
        }

        public static bool operator ==(Ray ray1, Ray ray2)
        {
            return ray1.origin == ray2.origin && ray1.direction == ray2.direction;
        }

        public static bool operator !=(Ray ray1, Ray ray2)
        {
            return !(ray1 == ray2);
        }

        public static Ray Reflect(Ray ray, Ray normal)
        {
            return new Ray(normal.origin, Vector3.Reflect(ray.direction, normal.direction));
        }

        public override string ToString()
        {
            return $"{origin}; {direction}";
        }

        public override bool Equals(object obj)
        {
            return obj is Ray ray &&
                   origin.Equals(ray.origin) &&
                   _direction.Equals(ray._direction);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(origin, _direction);
        }
    }
}
