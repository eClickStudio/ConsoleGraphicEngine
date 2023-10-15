using MathExtensions;
using RayTracingGraphicEngine3D.Abstract;
using RayTracingGraphicEngine3D.Components.Light.Abstract;
using RayTracingGraphicEngine3D.Components.Rendering;
using RayTracingGraphicEngine3D.Components.Rendering.Abstract;
using RayTracingGraphicEngine3D.Rays;
using RayTracingGraphicEngine3D.Rays.Intersections;
using RayTracingGraphicEngine3D.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.Scenes;
using RayTracingGraphicEngine3D.Tools;
using System;

namespace RayTracingGraphicEngine3D
{
    public class RayTracingGraphicEngine : AbstractGraphicEngine<RayTracingScene, IRayTracingCamera>
    {
        private Material _environmentMaterial;

        private float _minRayIntensity;

        /// <summary>
        /// Min intensity of ray; 
        /// The smaller value, the more detailed the frame will be
        /// Min value = 0.01f;
        /// Max value = 1;
        /// </summary>
        public float MinRayIntensity 
        { 
            get => _minRayIntensity;
            set
            {
                
                _minRayIntensity = MathExtension.Clamp(value, 0.01f, 1);
            }
        }

        //TODO: dont forget base fps in constructor
        public RayTracingGraphicEngine(Material environmentMaterial, float minRayIntensity) : base()
        {
            _environmentMaterial = environmentMaterial;
            MinRayIntensity = minRayIntensity;
        }


        public override void RenderFrame()
        {
            Material cameraEnvironment = camera.GetEnvironmentMaterial();

            for (int x = 0; x < camera.Resolution.X; x++)
            {
                for (int y = 0; y < camera.Resolution.Y; y++)
                {
                    Vector2Int screenPosition = new Vector2Int(x, y);

                    LightRay lightRay = camera.GetRay(screenPosition);

                    //TODO: check camera position and return right environment material
                    float pixelColor = RenderRay(cameraEnvironment, lightRay);
                    char pixelChar = camera.GetChar(pixelColor);

                    screen[x + y * camera.Resolution.X] = pixelChar;
                }
            }

            Console.Write(screen);
        }

        //TODO: test sky char maybe its redundant

        private float RenderRay(Material environmentMaterial, LightRay ray)
        {
            FullIntersection? intersection = GetNearestIntersection(ray.Ray);

            if (intersection.HasValue)
            {
                float absorptionCoefficient = GetAbsorptionCoefficient(environmentMaterial, intersection.Value.ShapeIntersection.MinIntersectionDistance);
                ray.Intensity /= absorptionCoefficient;

                if (ray.Intensity < MinRayIntensity)
                {
                    return 0;
                }

                if (intersection.Value.Intersectable is ILight light)
                {
                    return light.Intensity * ray.Intensity;
                }
                else if (intersection.Value.Intersectable is IShapeRenderer renderer)
                {
                    Ray normalRay = intersection.Value.ShapeIntersection.NormalRay;

                    LightRay reflectedRay = renderer.GetReflectedRay(environmentMaterial, ray, normalRay);
                    float reflectedRayIntensity = RenderRay(environmentMaterial, reflectedRay);

                    LightRay refractedRay = renderer.GetRefractedRay(environmentMaterial, ray, normalRay);
                    float refractedRayIntensity = RenderRay(renderer.Material, refractedRay);

                    return reflectedRayIntensity + refractedRayIntensity;
                }
            }

            return 0;
        }

        private float GetAbsorptionCoefficient(Material environmentMaterial, float distance)
        {
            return (float)Math.Pow(Math.E, environmentMaterial.AbsorptionRate * distance);
        }

        private float GetReflectedCoefficient(Material material1, Material material2)
        {
            float refractiveIndex1 = material1.RefractiveIndex;
            float refractiveIndex2 = material2.RefractiveIndex;

            return (float)Math.Pow((refractiveIndex1 - refractiveIndex2) / (refractiveIndex1 + refractiveIndex2), 2);
        }

        private float GetRefractedCoefficient(float reflectedCoefficient)
        {
            return 1 - reflectedCoefficient;
        }

        private FullIntersection? GetNearestIntersection(Ray ray)
        {
            IIntersectable nearestIntersectable = null;
            ShapeIntersection? nearestIntersection = null;

            foreach (IIntersectable intersectable in LocalScene.Intersectables)
            {
                IIntersectableShape shape = intersectable.Shape;
                ShapeIntersection? intersection = shape.GetShapeIntersection(ray);

                if (!nearestIntersection.HasValue 
                    || (intersection.HasValue && intersection.Value.MinIntersectionDistance < nearestIntersection.Value.MinIntersectionDistance))
                {
                    nearestIntersectable = intersectable;
                    nearestIntersection = intersection;
                }
            }

            if (nearestIntersectable != null)
            {
                return new FullIntersection(nearestIntersectable, nearestIntersection.Value);
            }

            return null;
        }
    }
}
