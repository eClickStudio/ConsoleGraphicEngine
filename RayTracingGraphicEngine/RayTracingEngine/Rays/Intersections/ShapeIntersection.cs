namespace RayTracingGraphicEngine3D.Rays.Intersections
{
    public readonly struct ShapeIntersection
    {
        public float MinIntersectionDistance { get; }
        public bool DidPassThroughtEnvironment { get; }
        public Ray NormalRay { get; }

        public ShapeIntersection(float intersectionDistance, bool didPassThroughtEnvironment, Ray normalRay)
        {
            MinIntersectionDistance = intersectionDistance;
            DidPassThroughtEnvironment = didPassThroughtEnvironment;
            NormalRay = normalRay;
        }
    }
}
