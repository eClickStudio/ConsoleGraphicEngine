using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Light
{
    internal interface IDirectionLight : ILight
    {
        /// <summary>
        /// The direction of light
        /// </summary>
        Vector3 direction { get; set; }
    }
}
