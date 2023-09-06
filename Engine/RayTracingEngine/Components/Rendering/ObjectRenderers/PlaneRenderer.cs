using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;
using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers
{
    internal class PlaneRenderer : ObjectRenderer
    {
        //TODO: end this

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
