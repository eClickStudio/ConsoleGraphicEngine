using RayTracingGraphicEngine3D.Rays;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;
using RayTracingGraphicEngine3D.Components.Rendering.Abstract;
using Quaternion = MathExtensions.Quaternion;
using RayTracingGraphicEngine3D.RayTracingEngine.Configurations;
using MathExtensions;
using System.Xml.Schema;

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

        

        public LightRay GetReflectedRay(Material environmentMaterial, LightRay lightRay, Ray normalRay)
        {
            float n1 = environmentMaterial.ReflectiveIndex;
            float n2 = Material.ReflectiveIndex;
            //TODO: refCoef fix;
            float reflectionCoefficient = (float)Math.Pow((n1 - n2) / (n1 + n2), 2);
            //float reflectionCoefficient = 0.9f;
            float reflectedRayIntensity = reflectionCoefficient * lightRay.Intensity;

            Vector3 incidenceDirection = -lightRay.Ray.Direction;
            Vector3 normalDirection = normalRay.Direction;

            float incidenceAngle = VectorExtension.GetAngleBetweenVectors(incidenceDirection, normalDirection);
            float rotationAngle = incidenceAngle * 2;

            Vector3 rotationAxis = Vector3.Cross(incidenceDirection, normalDirection);

            //TODO: fix it; Its not ok to do two reflected directions; its just a crutch because of rotation axis cross
            Vector3 reflectedDirection;
            Vector3 reflectedDirection1 = Quaternion.RotateVector(incidenceDirection, rotationAxis, rotationAngle);
            Vector3 reflectedDirection2 = Quaternion.RotateVector(incidenceDirection, rotationAxis, -rotationAngle);

            if (Math.Round(VectorExtension.GetAngleBetweenVectors(reflectedDirection1, normalDirection)) == Math.Round(incidenceAngle))
            {
                reflectedDirection = reflectedDirection1;
            }
            else if (Math.Round(VectorExtension.GetAngleBetweenVectors(reflectedDirection2, normalDirection)) == Math.Round(incidenceAngle))
            {
                reflectedDirection = reflectedDirection2;
            }
            else
            {
                throw new Exception(
                    $"Reflected rays is invalid; Angle should be angle = {incidenceAngle}\n" +
                    $"Angle between normal and reflected ray 1 = {VectorExtension.GetAngleBetweenVectors(reflectedDirection1, normalDirection)}\n" +
                    $"Angle between normal and reflected ray 2 = {VectorExtension.GetAngleBetweenVectors(reflectedDirection2, normalDirection)}"
                    );
            }

            return new LightRay(new Ray(normalRay.Origin, reflectedDirection), reflectedRayIntensity, lightRay.InteractionCount);
        }

        public LightRay GetRefractedRay(Material environmentMaterial, LightRay lightRay, Ray normalRay)
        {
            float n1 = environmentMaterial.ReflectiveIndex;
            float n2 = Material.ReflectiveIndex;
            float refractionCoefficient = (float)((4 * n1 * n2) / Math.Pow(n1 + n2, 2));
            //float refractionCoefficient = 0;
            float refractedRayIntensity = refractionCoefficient * lightRay.Intensity;

            Vector3 incidenceDirection = -lightRay.Ray.Direction;
            Vector3 normalDirection = normalRay.Direction;

            float incidenceAngle = VectorExtension.GetAngleBetweenVectors(incidenceDirection, normalDirection);
            float refractedAngle = (float)Math.Asin(MathExtension.Clamp((float)((n1 / n2) * Math.Sin(incidenceAngle)), -1, 1));
            float rotationAngle = (float)(incidenceAngle + Math.PI - refractedAngle);

            Vector3 rotationAxis = Vector3.Cross(incidenceDirection, normalDirection);

            //TODO: fix it; Its not ok to do two refracted directions; its just a crutch because of rotation axis cross
            Vector3 refractedDirection;
            Vector3 refractedDirection1 = Quaternion.RotateVector(incidenceDirection, rotationAxis, rotationAngle);
            Vector3 refractedDirection2 = Quaternion.RotateVector(incidenceDirection, rotationAxis, -rotationAngle);

            if (Math.Round(VectorExtension.GetAngleBetweenVectors(refractedDirection1, -normalDirection)) == Math.Round(refractedAngle))
            {
                refractedDirection = refractedDirection1;
            }
            else if (Math.Round(VectorExtension.GetAngleBetweenVectors(refractedDirection2, -normalDirection)) == Math.Round(refractedAngle))
            {
                refractedDirection = refractedDirection2;
            }
            else
            {
                throw new Exception(
                    $"Refracted rays is invalid; Angle should be angle = {refractedAngle}\n" +
                    $"Angle between -normal and refracted ray 1 = {VectorExtension.GetAngleBetweenVectors(refractedDirection1, -normalDirection)}\n" + 
                    $"Angle between -normal and refracted ray 2 = {VectorExtension.GetAngleBetweenVectors(refractedDirection2, -normalDirection)}"
                    );
            }

            Vector3 origin = normalRay.Origin - 2 * normalDirection * Configurations.MIN_RAY_STEP; 
            return new LightRay(new Ray(origin, refractedDirection), refractedRayIntensity, lightRay.InteractionCount);
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
    }
}
