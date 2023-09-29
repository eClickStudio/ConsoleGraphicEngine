using Engine3D.Components.Abstract;

namespace ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering
{
    public interface IRenderer : IComponent
    {
        /// <summary>
        /// Renderer material
        /// </summary>
        Material Material { get; set; }
    }
}
