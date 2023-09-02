using ConsoleGraphicEngine.Engine.Tools;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract
{
    internal interface IObjectRenderer
    {
        Material material { get; set; }

        Vector3? GetNearestIntersection(Ray ray);

        IReadOnlyList<float> GetIntersectionDistances(Ray ray);

        Ray? GetNormal(Ray ray);

        float GetBrightness(Vector3 normalDirection, Vector3 lightDirection);
    }
}
