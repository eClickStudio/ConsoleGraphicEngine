using Engine3D.Components.Abstract;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;
using System;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Light
{
    public class Light : AbstractComponent, ILight
    {
        private float _intensity;
        public float Intensity
        {
            get
            {
                return _intensity;
            }
            set
            {
                if (value < 0 || !value.IsNormal())
                {
                    throw new ArgumentException($"Intensivety of light is invalid;\n" +
                        $"Min = 0; Value you want to set {value}");
                }

                if (_intensity != value)
                {
                    _intensity = value;

                    OnChanged();
                }
            }
        }

        public Light(float intensity)
        {
            Intensity = intensity;
        }
    }
}
