using Engine3D.ChangeTriggers;
using Engine3D.Objects;
using System;

namespace Engine3D.Components.Abstract
{
    public abstract class AbstractComponent : AbstractChangebleUpdateble, IComponent
    {
        private IObject3D _parentObject;

        public IObject3D ParentObject 
        {
            get
            {
                return _parentObject;
            }
            set
            {
                if (_parentObject != value)
                {
                    _parentObject = value;

                    OnAttachedToObjectEvent?.Invoke(value);

                    OnChanged();
                }
            }
        }

        public event Action<IObject3D> OnAttachedToObjectEvent;
    }
}
