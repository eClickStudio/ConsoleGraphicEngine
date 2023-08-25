using ConsoleGraphicEngine.Engine.Tools;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract
{
    internal interface IObjectRenderer
    {
        IReadOnlyList<float> GetIntersectionDistances(Ray ray);

        Ray GetNormal(Vector3 position);

        float GetBrightness(Vector3 intersectionPosition, Vector3 lightDirection);
    }
}
