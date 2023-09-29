using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract
{
    public interface IObjectRenderer : IRenderer
    {
        Vector3? GetNearestIntersection(Ray ray);

        IReadOnlyList<float> GetIntersectionDistances(Ray ray);

        Ray? GetNormal(Ray ray);

        float GetBrightness(Vector3 normalDirection, Vector3 lightDirection);
    }
}
