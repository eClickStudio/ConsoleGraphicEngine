using ConsoleGraphicEngine.Engine.Objects.Abstract;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Abstract
{
    interface IComponent
    {
        IObject3D parentObject { get; }
    }
}
