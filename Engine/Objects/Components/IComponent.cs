namespace ConsoleGraphicEngine.Engine.Objects.Components
{
    interface IComponent
    {
        IObject3D parentObject { get; }
    }
}
