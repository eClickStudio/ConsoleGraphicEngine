using ConsoleGraphicEngine.Engine.Objects.Components;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;

namespace ConsoleGraphicEngine.Engine.Objects
{
    internal class VisibleObject : Object3D
    {
        public VisibleObject(string name = "noname_object") : base(name)
        {
            components.Add(new ObjectRenderer());
        }
    }
}
