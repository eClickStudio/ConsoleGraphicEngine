using System;

namespace ConsoleGraphicEngine.Engine.Basic.Abstract
{
    internal interface IChangeble
    {
        /// <summary>
        /// Triggers on change
        /// </summary>
        event Action onChanged;

        /// <summary>
        /// Call when you want to chek changes
        /// </summary>
        /// <returns>Are there changes?</returns>
        bool CheckChanges();
    }
}
