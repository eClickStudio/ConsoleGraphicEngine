using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering.Abstract;
using Quaternion = MathExtensions.Quaternion;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Configurations;

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

        public LightRay GetReflectedRay(LightRay lightRay, Ray normalRay)
        {
            float n1 = lightRay.EnvironmentMaterial.ReflectiveIndex;
            float n2 = Material.ReflectiveIndex;
            float reflectionCoefficient = (float)Math.Pow((n1 - n2) / (n1 + n2), 2);
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

            return new LightRay(new Ray(normalRay.Origin, reflectedDirection), reflectedRayIntensity, lightRay.EnvironmentMaterial, lightRay.InteractionCount);
        }

        public LightRay GetRefractedRay(LightRay lightRay, Ray normalRay)
        {
            float n1 = lightRay.EnvironmentMaterial.ReflectiveIndex;
            float n2 = Material.ReflectiveIndex;
            float refractionCoefficient = (float)((4 * n1 * n2) / Math.Pow(n1 + n2, 2));
            float refractedRayIntensity = refractionCoefficient * lightRay.Intensity;

            Vector3 incidenceDirection = -lightRay.Ray.Direction;
            Vector3 normalDirection = normalRay.Direction;

            float incidenceAngle = VectorExtension.GetAngleBetweenVectors(incidenceDirection, normalDirection);
            float refractedAngle = (float)Math.Asin(MathExtension.Clamp((float)((n1 / n2) * Math.Sin(incidenceAngle)), -1, 1));
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
            return new LightRay(new Ray(origin, refractedDirection), refractedRayIntensity, Material, lightRay.InteractionCount);
        }
    }
}
