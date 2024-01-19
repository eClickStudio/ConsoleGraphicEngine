using MathExtensions;
using System;

namespace RayTracingGraphicEngine3D.Components.Rendering
{
    /// <summary>
    /// Optical properties of object
    /// </summary>
    public struct Material
    {
        //This model is wrong: there are different results with different light direction

        //TODO: try to add roughness: add some angular offset to reflected and refracted rays
        //TODO: all parameters should work

        private float _reflectiveIndedex;

        /// <summary>
        /// The ratio of light speed in a vacuum to light speed in this material
        /// 1 - min value; density of vacuum
        /// 1.3 - water density
        /// float.Max value - max value; imposible density
        /// </summary>
        public float ReflectiveIndex
        {
            get => _reflectiveIndedex;
            set
            {
                _reflectiveIndedex = MathExtension.Clamp(value, 1, float.MaxValue);
            }
        }


        private float _absorptionRate;

        /// <summary>
        /// How intensely material absorbs light
        /// 0 - this material is completely transparent; light does not become weaker
        /// x - intensity of ray spreading in this material weakens by (e)^x times each unit; (e) - euler number ~= 2.72
        /// </summary>
        public float AbsorptionRate
        {
            get => _absorptionRate;
            set
            {
                _absorptionRate = MathExtension.Clamp(value, 0, float.MaxValue);
            }
        }

        /// <summary>
        /// Optical properties of object
        /// </summary>
        /// <param name="relativeLightSpeed">speed relative to speed in vacuum (max speed) max - 1; min 0</param>
        /// <param name="distanceToHalfRayIntensity">Distance after which the light intensivety will be halved</param>
        public Material(float relativeLightSpeed, float distanceToHalfRayIntensity)
        {
            relativeLightSpeed = MathExtension.Clamp(relativeLightSpeed, 0, 1);
            distanceToHalfRayIntensity = MathExtension.Clamp(distanceToHalfRayIntensity, 0, float.MaxValue);

            _reflectiveIndedex = 0;
            _absorptionRate = 0;

            if (relativeLightSpeed == 0)
            {
                _reflectiveIndedex = float.MaxValue;
            }
            else
            {
                _reflectiveIndedex = 1 / relativeLightSpeed;
            }


            if (distanceToHalfRayIntensity == 0)
            {
                _absorptionRate = float.MaxValue;
            }
            else
            {
                _absorptionRate = (float)(Math.Log(2, Math.E) / distanceToHalfRayIntensity);
            }
        }

        //TODO: distance to half ray intansity concrete

        public static Material Solid => new Material(0.6f, 0);
        public static Material Water => new Material(0.75f, 100);
        public static Material Diamond => new Material(0.41f, 1000);
        public static Material Glass => new Material(0.6f, 1000);
        public static Material Vacuum => new Material(1, float.MaxValue);
        public static Material Fog => new Material(1, 10);
    }
}
