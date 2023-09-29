using System;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering
{
    public struct Ray
    {
        public Vector3 Origin;

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
                    throw new ArgumentException("Direction of ray cannot be zero!");
                }

                _direction = Vector3.Normalize(value);
            }
        }

        public Ray(Vector3 direction)
        {
            Origin = new Vector3();

            _direction = new Vector3();
            this.Direction = direction;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.Origin = origin;

            _direction = new Vector3();
            this.Direction = direction;
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
