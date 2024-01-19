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
using System.Numerics;

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

                    //TODO: check camera position and return right environment material; May be ray should return result if it intersect inside or outside renderer shape
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
                ray.Interact();

                float absorptionCoefficient = GetAbsorptionCoefficient(environmentMaterial, intersection.Value.ShapeIntersection.MinIntersectionDistance);
                ray.Intensity /= absorptionCoefficient;

                //Console.WriteLine($"rayIntensity = {ray.Intensity}");

                if (ray.Intensity < MinRayIntensity)
                {
                    if (intersection.Value.Intersectable is IShapeRenderer && ray.Intensity > 0)
                    {
                        Console.WriteLine("Material absorbed whole light");
                    }

                    return 0;
                }

                if (intersection.Value.Intersectable is ILight light)
                {
                    //Console.WriteLine($"Found light ray.Intensity = {ray.Intensity}");

                    if (ray.Intensity < 1)
                    {
                        //Console.WriteLine("Reflected Ray found light");
                    }

                    return light.Intensity * ray.Intensity;
                }
                else if (intersection.Value.Intersectable is IShapeRenderer renderer)
                {
                    Ray normalRay = intersection.Value.ShapeIntersection.NormalRay;

                    LightRay reflectedRay = renderer.GetReflectedRay(environmentMaterial, ray, normalRay);
                    //Console.WriteLine($"Reflected Ray Intensity = {reflectedRay.Intensity}");
                    float reflectedRayIntensity = RenderRay(environmentMaterial, reflectedRay);

                    if (reflectedRayIntensity != 0)
                    {
                        //Console.WriteLine($"1) reflectedRayIntensity = {reflectedRayIntensity}");
                    }

                    //LightRay refractedRay = renderer.GetRefractedRay(environmentMaterial, ray, normalRay);
                    ////Console.WriteLine($"invirinment Renderer = {renderer.ParentObject.Name}");
                    //float refractedRayIntensity = RenderRay(renderer.Material, refractedRay);

                    //if (refractedRayIntensity != 0)
                    //{
                    //    //Console.WriteLine($"2) refractedRayIntensity = {refractedRayIntensity}");
                    //}

                    //return reflectedRayIntensity + refractedRayIntensity;

                    return reflectedRayIntensity;
                }
            }
            else if (ray.InteractionCount > 0)
            {
                IDirectionLight globalLight = LocalScene.GlobalLight;
                float directionLightIntensity = Vector3.Dot(-ray.Ray.Direction, globalLight.Direction) * globalLight.Intensity;

                return ray.Intensity + directionLightIntensity;
            }

            //Console.WriteLine("No intersections");
            return 0;
        }

        private float GetAbsorptionCoefficient(Material environmentMaterial, float distance)
        {
            return (float)Math.Pow(Math.E, environmentMaterial.AbsorptionRate * distance);
        }

        private FullIntersection? GetNearestIntersection(Ray ray)
        {
            IIntersectable nearestIntersectable = null;
            ShapeIntersection? nearestIntersection = null;

            foreach (IIntersectable intersectable in LocalScene.Intersectables)
            {
                IIntersectableShape shape = intersectable.Shape;
                ShapeIntersection? intersection = shape.GetShapeIntersection(ray);

                if (intersection.HasValue)
                {
                    if (!nearestIntersection.HasValue
                        || intersection.Value.MinIntersectionDistance < nearestIntersection.Value.MinIntersectionDistance)
                    {
                        nearestIntersectable = intersectable;
                        nearestIntersection = intersection;
                    }
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
