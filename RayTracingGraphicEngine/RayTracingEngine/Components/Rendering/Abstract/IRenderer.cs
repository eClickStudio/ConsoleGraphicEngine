namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering.Abstract
{
    public interface IRenderer
    {
        /// <summary>
        /// Renderer material
        /// </summary>
        Material Material { get; set; }
    }
}
