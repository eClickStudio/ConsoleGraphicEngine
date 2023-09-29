using System;

namespace ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering
{
    public struct Material
    {
        //TODO: add textures
        // add all parameters light, glass...

        public float Glow { get; private set; }
        public float Brightness { get; private set; }

        public Material(float brightness, float glow)
        {
            Brightness = Math.Clamp(brightness, 0, 1);
            Glow = Math.Clamp(glow, 0, 1);
        }

        public static Material Standart => new Material(0.9f, 0);
        public static Material Light => new Material(0, 1);
        public static Material Glass => new Material(1, 0);
        public static Material Transparent => new Material(0, 0);
    }
}
