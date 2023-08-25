using ConsoleGraphicEngine.Engine.Objects.Abstract;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Abstract
{
    internal abstract class Component : IComponent
    {
        public IObject3D parentObject { get; set; }
    }
}
