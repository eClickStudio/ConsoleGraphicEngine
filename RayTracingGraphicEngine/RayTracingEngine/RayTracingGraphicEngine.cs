using MathExtensions;
using RayTracingGraphicEngine3D.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Scenes;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using System;
using System.Numerics;

namespace RayTracingGraphicEngine3D
{
    public class RayTracingGraphicEngine : AbstractGraphicEngine<RayTracingScene, IRayTracingCamera>
    {
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
        public RayTracingGraphicEngine(float minRayIntensity) : base()
        {
            MinRayIntensity = minRayIntensity;
        }


        public override void RenderFrame()
        {
            for (int x = 0; x < camera.Resolution.X; x++)
            {
                for (int y = 0; y < camera.Resolution.Y; y++)
                {
                    Vector2Int screenPosition = new Vector2Int(x, y);

                    LightRay lightRay = camera.GetRay(screenPosition);

                    //TODO: check camera position and return right environment material; May be ray should return result if it intersect inside or outside renderer shape
                    float pixelColor = RenderRay(lightRay);
                    char pixelChar = camera.GetChar(pixelColor);

                    screen[x + y * camera.Resolution.X] = pixelChar;
                }
            }

            Console.Write(screen);
        }

        //TODO: test sky char maybe its redundant

        private float RenderRay(LightRay ray)
        {
            FullIntersection? intersection = GetNearestIntersection(ray.Ray);

            if (intersection.HasValue)
            {
                ray.Interact();

                float absorptionCoefficient = GetAbsorptionCoefficient(ray.EnvironmentMaterial, intersection.Value.ShapeIntersection.MinIntersectionDistance);
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

                    LightRay reflectedRay = renderer.GetReflectedRay(ray, normalRay);
                    float reflectedRayIntensity = RenderRay(reflectedRay);

                    LightRay refractedRay = renderer.GetRefractedRay(ray, normalRay);
                    float refractedRayIntensity = RenderRay(refractedRay);

                    return reflectedRayIntensity + refractedRayIntensity;
                }
            }
            else if (ray.InteractionCount > 0)
            {
                IDirectionLight globalLight = LocalScene.GlobalLight;
                float directionLightIntensity = Vector3.Dot(-ray.Ray.Direction, globalLight.WorldDirection) * globalLight.Intensity;

                return ray.Intensity + directionLightIntensity;
            }

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
