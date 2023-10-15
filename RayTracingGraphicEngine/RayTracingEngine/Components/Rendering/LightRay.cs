using System.Numerics;
using System;
using RayTracingGraphicEngine3D.Rays;

namespace RayTracingGraphicEngine3D.Components.Rendering
{
    //TODO: check how many iterations it does
    public struct LightRay
    {
        private float _intensity;

        /// <summary>
        /// Intensity of light;
        /// min - 0;
        /// max - float.MaxValue;
        /// </summary>
        public float Intensity
        {
            get => _intensity;
            set
            {
                if (value == float.NaN)
                {
                    throw new ArgumentException("LightRay intensity can not be NaN");
                }

                _intensity = MathExtension.Clamp(value, 0, float.MaxValue);
            }
        }

        public Ray Ray { get; }

        public LightRay(Ray ray, float intensity)
        {
            _intensity = 0;

            Ray = ray;
            Intensity = intensity;
        }

        public LightRay(Vector3 origin, Vector3 direction, float intensity)
        {
            _intensity = 0;

            Ray = new Ray(origin, direction);
            Intensity = intensity;
        }
    }
}
