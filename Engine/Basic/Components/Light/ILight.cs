using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Light
{
    internal interface ILight : IComponent
    {
        /// <summary>
        /// The intensivety of light; Must be >= 0
        /// </summary>
        float intensivety { get; set; }
    }
}
