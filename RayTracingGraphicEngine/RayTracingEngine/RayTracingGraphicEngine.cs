﻿using MathExtensions;
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

                    float pixelColor = 0;

                    try
                    {
                        pixelColor = RenderRay(lightRay);
                    }
                    catch (StackOverflowException)
                    {
                        Console.WriteLine("---------StackOverflow------------------------");
                        lightRay.Hierarchy.PrintHierarchy();
                    }

                    char pixelChar = camera.GetChar(pixelColor);

                    //if (lightRay.Hierarchy.AllChildrenCount > 1 && pixelChar == ' ')
                    //{
                    //    Console.WriteLine("---------Iteration------------------------");
                    //    Console.WriteLine($"Result intensity = {pixelColor}");
                    //    lightRay.Hierarchy.PrintHierarchy();
                    //}

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

                LightRay reflectedRay = null;
                LightRay refractedRay = null;

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
                    float intensity = 0;

                    if (ray.EnvironmentMaterial != renderer.Material)
                    {
                        reflectedRay = renderer.GetReflectedRay(ray, normalRay);
                        intensity += RenderRay(reflectedRay);
                    }

                    if (intersection.Value.ShapeIntersection.DidPassThroughtEnvironment 
                        && ray.EnvironmentMaterial.ReflectiveIndex != 0 
                        && renderer.Material.ReflectiveIndex != 0)
                    {
                        refractedRay = renderer.GetRefractedRay(ray, normalRay);
                        intensity += RenderRay(refractedRay);
                    }

                    ray.AddHierarchyChildren(reflectedRay, refractedRay);

                    return intensity;
                }
            }
            else if (ray.InteractionCount > 0)
            {
                IDirectionLight globalLight = LocalScene.GlobalLight;
                float directionLightIntensity = Vector3.Dot(-ray.Ray.Direction, globalLight.WorldDirection) * globalLight.Intensity;

                if (directionLightIntensity >= 0)
                {
                    return ray.Intensity + directionLightIntensity;
                }
                else
                {
                    return ray.Intensity;
                }
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
