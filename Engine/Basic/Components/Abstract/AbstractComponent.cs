using ConsoleGraphicEngine.Engine.Basic.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Objects;
using System;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Abstract
{
    internal abstract class AbstractComponent : AbstractChangeble, IComponent
    {

        private IObject3D _parentObject;

        public IObject3D parentObject 
        {
            get
            {
                return _parentObject;
            }
            set
            {
                if (parentObject == null)
                {
                    throw new ArgumentException("Component can not be attached to null object!");
                }

                if (_parentObject != value)
                {
                    _parentObject = value;

                    attachedToObject?.Invoke(value);

                    OnChanged();
                }
            }
        }

        /// <summary>
        /// Triggers on attached to another object
        /// </summary>
        protected event Action<IObject3D> attachedToObject;

        public virtual void Update() { }
    }
}
