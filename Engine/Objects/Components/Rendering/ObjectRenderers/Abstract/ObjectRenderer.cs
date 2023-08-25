using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract
{
    internal abstract class ObjectRenderer : Component, IObjectRenderer
    {
        public Material material;

        public ObjectRenderer()
        {
            material = Material.standart;
        }

        public float GetBrightness(Vector3 intersectionPosition, Vector3 lightDirection)
        {
            lightDirection = Vector3.Normalize(lightDirection);
            Ray normal = GetNormal(intersectionPosition);

            return Math.Clamp(
                (Vector3.Dot(normal.direction, lightDirection)) * material.reflection + material.brightness,
                1, 0);
        }

        public abstract IReadOnlyList<float> GetIntersectionDistances(Ray ray);
        public abstract Ray GetNormal(Vector3 position);
    }
}
