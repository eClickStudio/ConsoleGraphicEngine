using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering
{
    internal struct Intersection
    {
        public IObjectRenderer intersectedRenderer;
        public float intersectionDistance;

        public Intersection(in IObjectRenderer intersectedRenderer, float intersectionDistance)
        {
            this.intersectedRenderer = intersectedRenderer;
            this.intersectionDistance = intersectionDistance;
        }
    }
}
