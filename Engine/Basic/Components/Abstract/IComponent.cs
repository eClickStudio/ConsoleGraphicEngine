﻿using ConsoleGraphicEngine.Engine.Basic.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Objects;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Abstract
{
    interface IComponent : IChangebleUpdateble
    {
        /// <summary>
        /// The parent object of this component
        /// </summary>
        IObject3D parentObject { get; set; }
    }
}