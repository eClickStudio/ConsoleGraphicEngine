namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections
{
    public readonly struct ShapeIntersection
    {
        /// <summary>
        /// Distance to nearest intersection point
        /// </summary>
        public float MinIntersectionDistance { get; }

        /// <summary>
        /// Is ray pass through object (There are point of enter and point of exit)
        /// </summary>
        public bool DidPassThroughtEnvironment { get; }

        /// <summary>
        /// Ray normal for min intersection point
        /// </summary>
        public Ray NormalRay { get; }

        public ShapeIntersection(float intersectionDistance, bool didPassThroughtEnvironment, Ray normalRay)
        {
            MinIntersectionDistance = intersectionDistance;
            DidPassThroughtEnvironment = didPassThroughtEnvironment;
            NormalRay = normalRay;
        }
    }
}
