using System.Numerics;
using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers
{
    internal class CubeRenderer : BoxRenderer
    {
        public CubeRenderer(Material material, float edgeSize) : base(material, Vector3.One * edgeSize) { }
    }
}
