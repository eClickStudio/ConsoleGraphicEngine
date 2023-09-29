using System.Numerics;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers
{
    public class CubeRenderer : BoxRenderer
    {
        public CubeRenderer(Material material, float edgeSize) : base(material, Vector3.One * edgeSize) { }
    }
}
