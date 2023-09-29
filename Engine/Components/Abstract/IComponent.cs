using Engine3D.ChangeTriggers;
using Engine3D.Objects;
using System;

namespace Engine3D.Components.Abstract
{
    public interface IComponent : IChangebleUpdateble
    {
        /// <summary>
        /// The parent object of this component
        /// </summary>
        IObject3D ParentObject { get; set; }

        /// <summary>
        /// Triggers on attached to another object
        /// </summary>
        event Action<IObject3D> OnAttachedToObjectEvent;
    }
}
