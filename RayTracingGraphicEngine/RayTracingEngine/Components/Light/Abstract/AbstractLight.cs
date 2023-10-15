using Engine3D.Components.Abstract;
using System;

namespace RayTracingGraphicEngine3D.Components.Light.Abstract
{
    public class AbstractLight : AbstractComponent, ILight
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
                if (value < 0)
                {
                    throw new ArgumentException($"Intensivety of light is invalid; " +
                        $"Min = 0; Value you want to set {value}");
                }

                if (_intensity != value)
                {
                    _intensity = value;

                    OnChanged();
                }
            }
        }
    }
}
