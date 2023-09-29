using Engine3D.Components.Abstract;

namespace ConsoleGraphicEngine3D.Engine.Basic.Components.Light
{
    public interface ILight : IComponent
    {
        /// <summary>
        /// The intensivety of light; Must be >= 0
        /// </summary>
        float Intensivety { get; set; }
    }
}
