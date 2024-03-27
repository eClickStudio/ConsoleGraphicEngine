using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering.Abstract;
using Quaternion = MathExtensions.Quaternion;
using MathExtensions;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering
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

        private float GetReflectionCoefficient(float n1, float n2)
        {
            float reflectionCoefficient = (float)Math.Pow((n1 - n2) / (n1 + n2), 2);

            if (!MathExtension.IsNormal(reflectionCoefficient))
            {
                throw new ArithmeticException($"Reflection coefficient is incorrect; reflectionCoefficient = {reflectionCoefficient}; It should be normal");
            }

            return reflectionCoefficient;
        }

        public LightRay GetReflectedRay(LightRay lightRay, Ray normalRay)
        {
            float n1 = lightRay.EnvironmentMaterial.ReflectiveIndex;
            float n2 = Material.ReflectiveIndex;
            float reflectionCoefficient = GetReflectionCoefficient(n1, n2);
            float reflectedRayIntensity = reflectionCoefficient * lightRay.Intensity;

            Vector3 incidenceDirection = -lightRay.Ray.Direction;
            Vector3 normalDirection = normalRay.Direction;

            float incidenceAngle = VectorExtension.GetAngleBetweenVectors(incidenceDirection, normalDirection);
            float rotationAngle = incidenceAngle * 2;

            Vector3 rotationAxis = VectorExtension.GetRotationAxis(incidenceDirection, normalDirection, incidenceAngle);
            Vector3 reflectedDirection = Quaternion.RotateVector(incidenceDirection, rotationAxis, rotationAngle);

            if (Math.Round(VectorExtension.GetAngleBetweenVectors(reflectedDirection, normalDirection)) != Math.Round(incidenceAngle))
            {
                //TODO: delete this block
                //TEST: its just for tests

                throw new ArgumentException(
                    $"RotationAxis is wrong;\n" +
                    $"Angle between {reflectedDirection} and {normalDirection} = {Math.Round(VectorExtension.GetAngleBetweenVectors(reflectedDirection, normalDirection))};\n" +
                    $"Angle shold be = {Math.Round(incidenceAngle)};"
                );
            }

            return new LightRay(new Ray(normalRay.Origin, reflectedDirection), reflectedRayIntensity, lightRay.EnvironmentMaterial, lightRay.InteractionCount, "reflected", lightRay.Hierarchy.Parent, ParentObject.Name);
        }

        private float GetRefractionCoefficient(float n1, float n2)
        {
            float refractionCoefficient = 1 - GetReflectionCoefficient(n1, n2);

            if (!MathExtension.IsNormal(refractionCoefficient))
            {
                throw new ArithmeticException($"Refraction coefficient is incorrect; refractionCoefficient = {refractionCoefficient}; It should be normal");
            }

            return refractionCoefficient;
        }

        private float GetRefractedAngle(float n1, float n2, float incidenceAngle)
        {
            float refractedAngle = (float)Math.Asin(MathExtension.Clamp((float)((n1 / n2) * Math.Sin(incidenceAngle)), -1, 1));

            if (!MathExtension.IsNormal(refractedAngle))
            {
                throw new ArithmeticException($"Refracted angle is incorrect; refractedAngle = {refractedAngle}; It should be normal");
            }

            return refractedAngle;
        }

        public LightRay GetRefractedRay(LightRay lightRay, Ray normalRay)
        {
            float n1 = lightRay.EnvironmentMaterial.ReflectiveIndex;
            float n2 = Material.ReflectiveIndex;
            float refractionCoefficient = GetRefractionCoefficient(n1, n2);
            float refractedRayIntensity = refractionCoefficient * lightRay.Intensity;

            Vector3 incidenceDirection = -lightRay.Ray.Direction;
            Vector3 normalDirection = normalRay.Direction;

            float incidenceAngle = VectorExtension.GetAngleBetweenVectors(incidenceDirection, normalDirection);
            float refractedAngle = GetRefractedAngle(n1, n2, incidenceAngle);
            float rotationAngle = (float)(incidenceAngle + Math.PI - refractedAngle);

            Vector3 rotationAxis = VectorExtension.GetRotationAxis(incidenceDirection, normalDirection, incidenceAngle);
            Vector3 refractedDirection = Quaternion.RotateVector(incidenceDirection, rotationAxis, rotationAngle);

            if (Math.Round(VectorExtension.GetAngleBetweenVectors(refractedDirection, -normalDirection)) != Math.Round(refractedAngle))
            {
                //TODO: delete this block
                //TEST: its just for tests

                throw new ArgumentException(
                    $"RotationAxis is wrong;\n" +
                    $"Angle between {refractedDirection} and {-normalDirection} = {Math.Round(VectorExtension.GetAngleBetweenVectors(refractedDirection, -normalDirection))};\n" +
                    $"Angle shold be = {Math.Round(refractedAngle)};"
                    );
            }

            Vector3 origin = normalRay.Origin - 2 * normalDirection * Configurations.Configurations.MIN_RAY_STEP; 
            return new LightRay(new Ray(origin, refractedDirection), refractedRayIntensity, Material, lightRay.InteractionCount, "refracted", lightRay.Hierarchy.Parent, ParentObject.Name);
        }
    }
}
