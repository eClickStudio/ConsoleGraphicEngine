using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Tools
{
    internal struct Ray
    {
        public Vector3 startPosition;

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
            startPosition = new Vector3();

            _direction = new Vector3();
            this.direction = direction;
        }

        public Ray(Vector3 startPosition, Vector3 direction)
        {
            this.startPosition = startPosition;

            _direction = new Vector3();
            this.direction = direction;
        }

        public static bool operator ==(Ray ray1, Ray ray2)
        {
            return ray1.startPosition == ray2.startPosition && ray1.direction == ray2.direction;
        }

        public static bool operator !=(Ray ray1, Ray ray2)
        {
            return !(ray1 == ray2);
        }
    }
}
