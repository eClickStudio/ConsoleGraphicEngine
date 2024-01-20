namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections
{
    public readonly struct FullIntersection
    {
        public IIntersectable Intersectable { get; }
        public ShapeIntersection ShapeIntersection { get; }

        public FullIntersection(in IIntersectable intersectable, ShapeIntersection shapeIntersection)
        {
            Intersectable = intersectable;
            ShapeIntersection = shapeIntersection;
        }

        public FullIntersection(in IIntersectable intersectable, float intersectionDistance, bool didPassThroughtEnvironment, Ray normalRay)
        {
            Intersectable = intersectable;
            ShapeIntersection = new ShapeIntersection(intersectionDistance, didPassThroughtEnvironment, normalRay);
        }
    }
}
