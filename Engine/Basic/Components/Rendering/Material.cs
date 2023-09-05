using System;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Rendering
{
    struct Material
    {
        //TODO: add textures

        public float glow { get; private set; }
        public float brightness { get; private set; }

        public Material(float brightness, float glow)
        {
            this.brightness = Math.Clamp(brightness, 0, 1);
            this.glow = Math.Clamp(glow, 0, 1);
        }

        public static Material standart => new Material(0.9f, 0);
        public static Material light => new Material(0, 1);
        public static Material glass => new Material(1, 0);
        public static Material transparent => new Material(0, 0);
    }
}
