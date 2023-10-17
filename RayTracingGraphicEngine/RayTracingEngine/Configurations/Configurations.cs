namespace RayTracingGraphicEngine3D.RayTracingEngine.Configurations
{
    internal static class Configurations
    {
        //TODO: save config in xml or js file

        //TODO: may be MIN_RAY_STEP is redundant

        /// <summary>
        /// necessary, since a ray reflected from a surface without displacement intersects the same surface
        /// </summary>
        public const float MIN_RAY_STEP = 0;
    }
}
