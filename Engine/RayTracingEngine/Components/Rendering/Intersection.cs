using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;

namespace ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering
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
