using System;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers
{
    struct Material
    {
        public Material(float brightness, float glow)
        {
            this.brightness = Math.Clamp(brightness, 0, 1);
            this.glow = Math.Clamp(glow, 0, 1);
        }

        public static Material standart => new Material(0.8f, 0);
        public static Material light => new Material(0, 1);
        public static Material glass => new Material(1, 0);
        public static Material transparent => new Material(0, 0);

        public float glow { get; private set; }
        public float brightness { get; private set; }
    }
}
