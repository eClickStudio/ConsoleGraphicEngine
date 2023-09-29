using ConsoleGraphicEngine3D.Engine.Basic.Components.Light;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Light
{
    public class DirectionLight : AbstractLight, IDirectionLight
    {
        //TODO: light direction must be depended on transform rotation

        private Vector3 _direction = new Vector3(0, -1, 0);
        public Vector3 Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (value == Vector3.Zero)
                {
                    throw new ArgumentException($"Direction of light is invalid; " +
                        $"Value can not be = (0, 0, 0); Value you want to set {value}");
                }

                if (_direction != value)
                {
                    _direction = Vector3.Normalize(value);

                    OnChanged();
                }
            }
        }

        public DirectionLight(Vector3 direction, float intensivety)
        {
            Direction = direction;
            Intensivety = intensivety;
        }
    }
}
