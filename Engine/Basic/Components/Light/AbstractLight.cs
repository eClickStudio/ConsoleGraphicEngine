using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;
using System;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Light
{
    internal class AbstractLight : AbstractComponent, ILight
    {
        private float _intensivety = 1;
        public float intensivety
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
