using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers
{
    internal class PlaneRenderer : ObjectRenderer
    {
        public PlaneRenderer(Material material) : base(material)
        {

        }

        public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        {
            throw new NotImplementedException();
        }

        public override Ray? GetNormal(Ray ray)
        {
            throw new NotImplementedException();
        }
    }
}
