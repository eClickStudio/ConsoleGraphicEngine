using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Basic.Abstract
{
    internal abstract class AbstractChangebleUpdateble : IChangebleUpdateble
    {
        //TODO: fix changeble mechanic does not work

        protected bool changeTrigger { get; private set; }

        public event Action onChanged;

        protected IReadOnlyList<IChangebleUpdateble> changableUpdatebleChildren { get; set; }

        public bool Update()
        {
            SubUpdate();

            bool didChildrenChange = UpdateChildren();
            bool didChange = false;

            if (changeTrigger)
            {
                changeTrigger = false;

                didChange = true;
            }

            return didChange || didChildrenChange;
        }

        /// <summary>
        /// Update method. Calls after main update
        /// </summary>
        protected virtual void SubUpdate() { }

        private bool UpdateChildren()
        {
            bool didChange = false;

            if (changableUpdatebleChildren != null)
            {
                foreach (IChangebleUpdateble child in changableUpdatebleChildren)
                {
                    if (child.Update())
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
            changeTrigger = true;

            onChanged?.Invoke();
        }
    }
}
