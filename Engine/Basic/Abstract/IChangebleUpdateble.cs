using System;

namespace ConsoleGraphicEngine.Engine.Basic.Abstract
{
    internal interface IChangebleUpdateble
    {
        /// <summary>
        /// Triggers on change
        /// </summary>
        event Action onChanged;

        /// <summary>
        /// Update
        /// </summary>
        /// <returns>Are there changes?</returns>
        bool Update();
    }
}
