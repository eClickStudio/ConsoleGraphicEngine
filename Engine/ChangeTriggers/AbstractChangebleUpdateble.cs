using System;
using System.Collections.Generic;

namespace Engine3D.ChangeTriggers
{
    public abstract class AbstractChangebleUpdateble : IChangebleUpdateble
    {
        //TODO: fix changeble mechanic does not work

        protected bool ChangeTrigger { get; private set; }

        public event Action OnChangedEvent;

        protected IReadOnlyList<IChangebleUpdateble> ChangableUpdatebleChildren { get; set; }

        public bool Update(uint frameTime)
        {
            SubUpdate(frameTime);

            bool didChildrenChange = UpdateChildren(frameTime);
            bool didChange = false;

            if (ChangeTrigger)
            {
                ChangeTrigger = false;

                didChange = true;
            }

            return didChange || didChildrenChange;
        }

        /// <summary>
        /// Update method. Calls before main update
        /// </summary>
        /// <param name="frameTime">Time elapsed from last frame in milliseconds</param>
        protected virtual void SubUpdate(uint frameTime) { }

        private bool UpdateChildren(uint frameTime)
        {
            bool didChange = false;

            if (ChangableUpdatebleChildren != null)
            {
                foreach (IChangebleUpdateble child in ChangableUpdatebleChildren)
                {
                    if (child.Update(frameTime))
                    {
                        didChange = true;
                    }
                }
            }

            return didChange;
        }

        /// <summary>
        /// Call when something has update
        /// </summary>
        protected void OnChanged()
        {
            ChangeTrigger = true;

            OnChangedEvent?.Invoke();
        }
    }
}
