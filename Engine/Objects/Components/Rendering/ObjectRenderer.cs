using System;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering
{
    class ObjectRenderer : Component
    {
        public Material material;

        public ObjectRenderer()
        {
            material = Material.standart;
        }
    }
}
