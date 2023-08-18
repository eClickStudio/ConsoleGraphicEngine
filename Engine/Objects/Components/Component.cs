namespace ConsoleGraphicEngine.Engine.Objects.Components
{
    internal abstract class Component : IComponent
    {
        public IObject3D parentObject { get; set; }
    }
}
