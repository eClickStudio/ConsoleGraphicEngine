using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
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

        public Material material { get; set; }

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
        public abstract Ray? GetNormal(Ray ray);

        public Vector3? GetNearestIntersection(Ray ray)
        {
            IReadOnlyList<float> intersectionDistances = GetIntersectionDistances(ray);

            if (intersectionDistances == null || intersectionDistances.Count == 0)
            {
                return null;
            }

            float minDistance = float.MaxValue;
            foreach (float intersectionDistance in intersectionDistances)
            {
                if (intersectionDistance < minDistance)
                {
                    minDistance = intersectionDistance;
                }
            }

            return ray.origin + ray.direction * minDistance;
        }
    }
}
