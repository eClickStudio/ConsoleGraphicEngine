using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Rendering
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
                if (_intensivety != value && value >= 0)
                {
                    _intensivety = value;

                    OnChanged();
                }
            }
        }
    }
}
