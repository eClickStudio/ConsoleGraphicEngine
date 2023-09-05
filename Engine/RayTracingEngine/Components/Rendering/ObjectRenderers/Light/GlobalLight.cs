using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.RayTracing.Components.Rendering.ObjectRenderers.Light
{
    internal class GlobalLight : Component
    {
        //TODO: light direction must be depended on transform rotation

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
                    throw new ArgumentException("Direction of light cannot be zero!");
                }

                _direction = Vector3.Normalize(value);
            }
        }

        private float _intensivety;
        public float intensivety
        {
            get
            {
                return _intensivety;
            }
            set
            {
                _intensivety = Math.Clamp(value, 0, 1);
            }
        }

        public GlobalLight(Vector3 lightDirection, float lightIntensivety)
        {
            direction = lightDirection;
            intensivety = lightIntensivety;
        }
    }
}
