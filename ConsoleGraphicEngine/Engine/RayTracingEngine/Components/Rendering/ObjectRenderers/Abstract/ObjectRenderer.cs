using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using Engine3D.Components.Abstract;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract
{
    public abstract class ObjectRenderer : AbstractComponent, IObjectRenderer
    {
        protected const float _MIN_RAY_STEP = 0.01f;

        public Material Material { get; set; }

        public ObjectRenderer(Material material)
        {
            Material = material;
        }

        public float GetBrightness(Vector3 normalDirection, Vector3 lightDirection)
        {
            lightDirection = Vector3.Normalize(lightDirection);
            normalDirection = Vector3.Normalize(normalDirection);

            //TODO: real light color use all material coeff

            return Math.Clamp(
                (1 - Vector3.Dot(normalDirection, lightDirection)) * Material.AbsorptionRate,
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

            return ray.Origin + ray.Direction * minDistance;
        }
    }
}
