using System;

namespace ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering
{
    /// <summary>
    /// Optical properties of object
    /// </summary>
    public struct Material
    {
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
                _reflectiveIndedex = Math.Clamp(value, 1, float.MaxValue);
            }
        }


        private float _absorptionRate;

        /// <summary>
        /// How intensely material absorbs light
        /// 0 - this material is completely transparent
        /// x - intensity of ray spreading in this material weakens by (e)^x times each unit; (e) - euler number ~= 2.72
        /// </summary>
        public float AbsorptionRate
        {
            get => _absorptionRate;
            set
            {
                _absorptionRate = Math.Clamp(value, 0, float.MaxValue);
            }
        }

        /// <summary>
        /// Optical properties of object
        /// </summary>
        /// <param name="relativeLightSpeed">speed relative to speed in vacuum (max speed) max - 1; min 0</param>
        /// <param name="distanceToHalfRayIntensity">Distance after which the light intensivety will be halved</param>
        public Material(float relativeLightSpeed, float distanceToHalfRayIntensity)
        {
            relativeLightSpeed = Math.Clamp(relativeLightSpeed, 0, 1);
            distanceToHalfRayIntensity = Math.Clamp(distanceToHalfRayIntensity, 0, float.MaxValue);

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
        public static Material Diamons => new Material(0.41f, 1000);
        public static Material Glass => new Material(0.6f, 1000);
        public static Material Vacuum => new Material(1, float.MaxValue);
    }


    //public struct Material
    //{
    //    //TODO: all parameters should work

    //    private float _brightness;

    //    /// <summary>
    //    /// Color in current context
    //    /// 0 - black
    //    /// 1 - white
    //    /// </summary>
    //    public float Brightness 
    //    {
    //        get => _brightness;
    //        set
    //        {
    //            _brightness = Math.Clamp(value, 0, 1);
    //        }
    //    }


    //    private float _reflection;

    //    /// <summary>
    //    /// The ability of material to reflect rays
    //    /// 0 - full mate
    //    /// 1 - mirror
    //    /// </summary>
    //    public float Reflection
    //    {
    //        get => _reflection;
    //        set
    //        {
    //            _reflection = Math.Clamp(value, 0, 1);
    //        }
    //    }


    //    private float _transparency;

    //    /// <summary>
    //    /// The ability of material to transmit rays throught itself
    //    /// 0 - not transparent
    //    /// 1 - full transparent
    //    /// </summary>
    //    public float Transparency
    //    {
    //        get => _transparency;
    //        set
    //        {
    //            _transparency = Math.Clamp(value, 0, 1);
    //        }
    //    }


    //    private float _glowness;

    //    /// <summary>
    //    /// The ability of material to be light
    //    /// 0 - not light
    //    /// 0.5 - half intensivety light
    //    /// 1 - light
    //    /// </summary>
    //    public float Glow
    //    {
    //        get => _glowness;
    //        set
    //        {
    //            _glowness = Math.Clamp(value, 0, 1);
    //        }
    //    }

    //    public Material(float brightness, float reflection, float transparency, float glow)
    //    {
    //        _brightness = Math.Clamp(brightness, 0, 1);
    //        _reflection = Math.Clamp(reflection, 0, 1);
    //        _transparency = Math.Clamp(transparency, 0, 1);
    //        _glowness = Math.Clamp(glow, 0, 1);
    //    }

    //    public static Material Standart => new Material(0.9f, 0.1f, 0, 0);
    //    public static Material Light => new Material(0, 0, 0, 1);
    //    public static Material Glass => new Material(0, 1, 0.8f, 0);
    //    public static Material Transparent => new Material(0, 0, 1, 0);
    //}
}
