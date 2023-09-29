using System;

namespace Engine3D.ChangeTriggers
{
    public interface IChangebleUpdateble
    {
        /// <summary>
        /// Triggers on change
        /// </summary>
        event Action OnChangedEvent;

        //TODO: remove frameTime here because it needed only in graphic Engine

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="frameTime">millisecondsTime</param>
        /// <returns>Are there changes?</returns>
        bool Update(uint frameTime);
    }
}
