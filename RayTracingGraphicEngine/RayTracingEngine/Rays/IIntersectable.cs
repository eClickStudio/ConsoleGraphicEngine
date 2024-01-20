namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays
{
    public interface IIntersectable
    {
        /// <summary>
        /// Shape of this intersectable
        /// </summary>
        IIntersectableShape Shape { get; set; }
    }
}
