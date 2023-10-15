using RayTracingGraphicEngine3D.Rays;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;
using RayTracingGraphicEngine3D.Components.Rendering.Abstract;

namespace RayTracingGraphicEngine3D.Components.Rendering
{
    public class ShapeRenderer : AbstractComponent, IShapeRenderer
    {
        public Material Material { get; set; }

        public IIntersectableShape Shape { get; set; }

        public ShapeRenderer(in IIntersectableShape shape, Material material)
        {
            Material = material;
            Shape = shape;

            shape.onChanged += () => OnChanged();
        }

        //TODO: check how normal vector directed; It 

        public LightRay GetReflectedRay(Material environmentMaterial, LightRay lightRay, Ray normalRay)
        {
            float n1 = environmentMaterial.RefractiveIndex;
            float n2 = Material.RefractiveIndex;

            float reflectionCoefficient = (float)Math.Pow((n1 - n2) / (n1 + n2), 2);

            Vector3 rotationAxis = Vector3.Cross(-lightRay.Ray.Direction, normalRay.Direction);
        }

        public LightRay GetRefractedRay(Material environmentMaterial, LightRay lightRay, Ray normalRay)
        {
            float n1 = environmentMaterial.RefractiveIndex;
            float n2 = Material.RefractiveIndex;

            float refractionCoefficient = (float)((4 * n1 * n2) / Math.Pow(n1 + n2, 2));
        }

        //protected const float _MIN_RAY_STEP = 0.01f;

        //public Material Material { get; set; }

        //public ObjectRenderer(Material material)
        //{
        //    Material = material;
        //}

        ////public float GetBrightness(Vector3 normalDirection, Vector3 lightDirection)
        ////{
        ////    lightDirection = Vector3.Normalize(lightDirection);
        ////    normalDirection = Vector3.Normalize(normalDirection);

        ////    //TODO: real light color use all material coeff

        ////    return MathExtension.Clamp(
        ////        (1 - Vector3.Dot(normalDirection, lightDirection)) * Material.AbsorptionRate,
        ////        0, 1);
        ////}

        //public abstract IReadOnlyList<float> GetIntersectionDistances(Ray ray);
        //public abstract Ray GetNormal(Ray ray);

        //public Vector3? GetNearestIntersection(Ray ray)
        //{
        //    IReadOnlyList<float> intersectionDistances = GetIntersectionDistances(ray);

        //    if (intersectionDistances == null || intersectionDistances.Count == 0)
        //    {
        //        return null;
        //    }

        //    float minDistance = float.MaxValue;
        //    foreach (float intersectionDistance in intersectionDistances)
        //    {
        //        if (intersectionDistance < minDistance)
        //        {
        //            minDistance = intersectionDistance;
        //        }
        //    }

        //    return ray.Origin + ray.Direction * minDistance;
        //}

        //public LightRay GetReflectedRay(LightRay lightRay)
        //{
        //    throw new NotImplementedException();
        //}

        //public LightRay GetRefractedRay(LightRay lightRay)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
