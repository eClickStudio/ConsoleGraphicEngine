using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.Basic.Components.Light
{
    public interface IDirectionLight : ILight
    {
        /// <summary>
        /// The direction of light
        /// </summary>
        Vector3 Direction { get; set; }
    }
}
