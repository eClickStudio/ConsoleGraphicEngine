using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract
{
    internal abstract class ObjectRenderer : Component, IObjectRenderer
    {
        protected const float _RAY_STEP = 0.01f;

        public Material material;

        public ObjectRenderer(Material material)
        {
            this.material = material;
        }

        public float GetBrightness(Vector3 normalDirection, Vector3 lightDirection)
        {
            lightDirection = Vector3.Normalize(lightDirection);
            normalDirection = Vector3.Normalize(normalDirection);

            return Math.Clamp(
                (1 - Vector3.Dot(normalDirection, lightDirection)) * material.brightness + material.glow,
                0, 1);
        }

        public abstract IReadOnlyList<float> GetIntersectionDistances(Ray ray);
        public abstract Ray GetNormal(Vector3 position);
    }
}
