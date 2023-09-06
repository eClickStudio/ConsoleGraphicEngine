using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract
{
    internal interface IObjectRenderer : IRenderer
    {
        Vector3? GetNearestIntersection(Ray ray);

        IReadOnlyList<float> GetIntersectionDistances(Ray ray);

        Ray? GetNormal(Ray ray);

        float GetBrightness(Vector3 normalDirection, Vector3 lightDirection);
    }
}
