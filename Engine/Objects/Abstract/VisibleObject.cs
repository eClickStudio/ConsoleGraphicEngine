using ConsoleGraphicEngine.Engine.Objects.Components;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;

namespace ConsoleGraphicEngine.Engine.Objects
{
    internal class VisibleObject : Object3D
    {
        public ObjectRenderer renderer { get; private set; }

        public VisibleObject(in ObjectRenderer renderer, string name = "noname_object") : base(name) 
        {
            this.renderer = renderer;
            renderer.parentObject = this;
        }
    }
}
