using System;

namespace ConsoleGraphicEngine.Engine.Basic.Abstract
{
    internal abstract class AbstractChangeble : IChangeble
    {
        protected bool changeTrigger { get; private set; }

        public event Action onChanged;

        public bool CheckChanges()
        {
            if (changeTrigger)
            {
                changeTrigger = false;

                onChanged?.Invoke();
            }

            return changeTrigger;
        }

        /// <summary>
        /// Call when something has update
        /// </summary>
        protected void OnChanged()
        {
            changeTrigger = true;
        }
    }
}
