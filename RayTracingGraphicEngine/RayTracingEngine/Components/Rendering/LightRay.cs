using System.Numerics;
using System;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using MathExtensions;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering
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
                if (!value.IsNormal())
                {
                    throw new ArgumentException("LightRay intensity can not be NaN");
                }

                _intensity = MathExtension.Clamp(value, 0, float.MaxValue);
            }
        }

        public Ray Ray { get; }

        /// <summary>
        /// How many times ray has been reflected or refracted;
        /// </summary>
        public uint InteractionCount { get; private set; }

        public Material EnvironmentMaterial { get; }

        public LightRay(Ray ray, float intensity, Material environmentMaterial, uint interactionCount = 0)
        {
            _intensity = 0;

            InteractionCount = interactionCount;
            EnvironmentMaterial = environmentMaterial;
            Ray = ray;
            Intensity = intensity;
        }

        public LightRay(Vector3 origin, Vector3 direction, float intensity, Material environmentMaterial, uint interactionCount = 0)
        {
            _intensity = 0;

            InteractionCount = interactionCount;
            EnvironmentMaterial = environmentMaterial;
            Ray = new Ray(origin, direction);
            Intensity = intensity;
        }

        public void Interact()
        {
            InteractionCount++;
        }
    }
}
