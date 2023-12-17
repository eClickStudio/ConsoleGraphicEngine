using RayTracingGraphicEngine3D.Rays;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;
using RayTracingGraphicEngine3D.Components.Rendering.Abstract;
using Quaternion = MathExtensions.Quaternion;
using RayTracingGraphicEngine3D.RayTracingEngine.Configurations;

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

            shape.OnChangedEvent += () => OnChanged();
        }

        //TODO: check how normal vector directed; It 

        private float GetAngleBetweenVectors(Vector3 a, Vector3 b)
        {
            if (Math.Round(a.Length(), 1) != 1 || Math.Round(b.Length(), 1) != 1)
            {
                //TODO: its just for tests
                throw new ArgumentException($"Vectors are not normalized; a = {a}; b = {b};" +
                    $"\nNormalized a = {Vector3.Normalize(a)}; Normalized b = {Vector3.Normalize(b)};" +
                    $"\n a Lenght = {a.Length()}; b Lenght = {b.Length()}");
            }

            return (float)Math.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
        }

        public LightRay GetReflectedRay(Material environmentMaterial, LightRay lightRay, Ray normalRay)
        {
            float n1 = environmentMaterial.RefractiveIndex;
            float n2 = Material.RefractiveIndex;
            float reflectionCoefficient = (float)Math.Pow((n1 - n2) / (n1 + n2), 2);
            float reflectedRayIntensity = reflectionCoefficient * lightRay.Intensity;

            Vector3 incidenceDirection = -lightRay.Ray.Direction;
            Vector3 normalDirection = normalRay.Direction;

            float incidenceAngle = GetAngleBetweenVectors(incidenceDirection, normalDirection);
            float rotationAngle = incidenceAngle * 2;

            Vector3 rotationAxis = Vector3.Cross(incidenceDirection, normalDirection);
            Vector3 reflectedDirection = Quaternion.RotateVector(incidenceDirection, rotationAxis, rotationAngle);

            //TEST: just for tests
            if (Math.Round(GetAngleBetweenVectors(reflectedDirection, normalDirection), 2) != Math.Round(incidenceAngle, 2))
            {
                throw new Exception($"Reflected ray is invalid; Angle between normal and reflected rays = {GetAngleBetweenVectors(incidenceDirection, reflectedDirection)}");
            }

            return new LightRay(new Ray(normalRay.Origin, reflectedDirection), reflectedRayIntensity);
        }

        public LightRay GetRefractedRay(Material environmentMaterial, LightRay lightRay, Ray normalRay)
        {
            float n1 = environmentMaterial.RefractiveIndex;
            float n2 = Material.RefractiveIndex;
            float refractionCoefficient = (float)((4 * n1 * n2) / Math.Pow(n1 + n2, 2));
            float refractedRayIntensity = refractionCoefficient * lightRay.Intensity;

            Vector3 incidenceDirection = -lightRay.Ray.Direction;
            Vector3 normalDirection = normalRay.Direction;

            float incidenceAngle = GetAngleBetweenVectors(incidenceDirection, normalDirection);
            float refractedAngle = (float)Math.Asin(n1 / n2 * Math.Sin(incidenceAngle));
            float rotationAngle = (float)(incidenceAngle + Math.PI - refractedAngle);

            Vector3 rotationAxis = Vector3.Cross(incidenceDirection, normalDirection);
            Vector3 refractedDirection = Quaternion.RotateVector(incidenceDirection, rotationAxis, rotationAngle);

            //TEST: just for tests
            if (Math.Round(GetAngleBetweenVectors(refractedDirection, -normalDirection), 2) != Math.Round(refractedAngle, 2))
            {
                throw new Exception($"Refracted ray is invalid; Angle between normal and refracted rays = {GetAngleBetweenVectors(refractedDirection, -normalDirection)}");
            }

            Vector3 origin = normalRay.Origin - 2 * normalDirection * Configurations.MIN_RAY_STEP; 
            return new LightRay(new Ray(origin, refractedDirection), refractedRayIntensity);
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
