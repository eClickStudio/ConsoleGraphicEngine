using Engine3D.Components.Abstract;
using System;

namespace ConsoleGraphicEngine3D.Engine.Basic.Components.Light
{
    public class AbstractLight : AbstractComponent, ILight
    {
        private float _intensivety = 1;
        public float Intensivety
        {
            get
            {
                return _intensivety;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"Intensivety of light is invalid; " +
                        $"Min = 0; Value you want to set {value}");
                }

                if (_intensivety != value)
                {
                    _intensivety = value;

                    OnChanged();
                }
            }
        }
    }
}
