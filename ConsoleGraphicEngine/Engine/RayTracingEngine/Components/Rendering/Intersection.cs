using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering
{
    public struct Intersection
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
