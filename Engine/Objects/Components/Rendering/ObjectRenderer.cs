using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering
{
    internal abstract class ObjectRenderer : Component
    {
        public Material material;

        public ObjectRenderer()
        {
            material = Material.standart;
        }

        public abstract IReadOnlyList<Vector3> Intersect(Ray ray);
    }
}
