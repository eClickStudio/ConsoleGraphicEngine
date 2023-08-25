using System;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers
{
    struct Material
    {
        public Material(float reflection, float transparency, float brightness)
        {
            this.reflection = Math.Clamp(reflection, 0, 1);
            this.transparency = Math.Clamp(transparency, 0, 1);
            this.brightness = Math.Clamp(brightness, 0, 1);
        }

        public static Material standart => new Material(0, 0, 0);
        public static Material light => new Material(0, 0, 1);
        public static Material glass => new Material(0, 0.5f, 0);
        public static Material transparent => new Material(0, 1, 0);
        public static Material mirror => new Material(1, 0, 0);

        public float reflection { get; private set; }
        public float transparency { get; private set; }
        public float brightness { get; private set; }
    }
}
