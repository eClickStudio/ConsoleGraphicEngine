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
using System.Security;
using System.Collections.Generic;

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

        //DEBUG:
        private int frameNum = 0;
        private int requiredFrameNum = 1000;

        private Vector2Int requiredPixel1 = new Vector2Int(96, 30);
        private Vector2Int requiredPixel2 = new Vector2Int(140, 30);
        private Vector2Int centerPixel = new Vector2Int(118, 31);

        public override void RenderFrame()
        {
            //DEBUG
            frameNum++;
            Dictionary<Vector2Int, RayDebugData> catchedRays = new Dictionary<Vector2Int, RayDebugData>();
            Vector3 startRayDirection;

            for (int x = 0; x < camera.Resolution.X + 1; x++)
            {
                for (int y = 0; y < camera.Resolution.Y; y++)
                {
                    char pixelChar;

                    if (x == camera.Resolution.X)
                    {
                        pixelChar = '\n';
                    }
                    else
                    {
                        Vector2Int screenPosition = new Vector2Int(x, y);


                        LightRay lightRay = camera.GetRay(screenPosition);
                        startRayDirection = lightRay.Ray.Direction;

                        //DEBUG
                        if (frameNum == requiredFrameNum &&
                            screenPosition == requiredPixel1)
                        {

                        }

                        //TODO: check camera position and return right environment material; May be ray should return result if it intersect inside or outside renderer shape

                        float pixelColor = 0;

                        try
                        {
                            pixelColor = RenderRay(lightRay);
                        }
                        catch (StackOverflowException)
                        {
                            //DEBUG: why didn't catch
                            Console.WriteLine("---------StackOverflow------------------------");
                            lightRay.Hierarchy.PrintHierarchy();
                        }

                        pixelChar = camera.GetChar(pixelColor);

                        //DEBUG: 
                        if (frameNum == requiredFrameNum)
                        {
                            if (GetDistanceBetweenScreenPositions(screenPosition, requiredPixel1) == 1 || 
                                GetDistanceBetweenScreenPositions(screenPosition, requiredPixel2) == 1)
                            {
                                pixelChar = 'ё';
                            }

                            if (screenPosition == centerPixel)
                            {
                                pixelChar = 'Ё';   
                            }

                            if (screenPosition == requiredPixel1 ||
                                screenPosition == requiredPixel2 ||
                                screenPosition == centerPixel)
                            {
                                catchedRays.Add(new Vector2Int(x, y), new RayDebugData(startRayDirection, lightRay, pixelColor, pixelChar));
                            }
                        }

                        screen[x + y * camera.Resolution.X] = pixelChar;

                        //DEBUG:
                        PrintDebugScreenCoordinates(x, y);
                    }
                }
            }

            Console.Write(screen);


            //DEBUG:
            if (frameNum == requiredFrameNum)
            {
                Console.WriteLine($"CatchedFrame num = {frameNum}");

                Console.WriteLine();
                Console.WriteLine("GlobalLight----------------------------------------------");
                Console.WriteLine($"Intensity = {LocalScene.GlobalLight.Intensity}");

                Console.WriteLine();
                foreach (KeyValuePair<Vector2Int, RayDebugData> pair in catchedRays)
                {
                    Vector2Int screenPosition = pair.Key;
                    LightRay lightRay = pair.Value.lightRay;
                    float pixelColor = pair.Value.pixelColor;
                    char pixelChar = pair.Value.pixelChar;


                    Console.WriteLine("Check-Pixels----------------------------------------------");

                    Console.WriteLine();
                    Console.WriteLine($"ScreenPosition = {screenPosition}");
                    Console.WriteLine($"StartRayDirection = {pair.Value.startRayDirection}");
                    Console.WriteLine($"ResultIntensity = {pixelColor}");
                    Console.WriteLine($"ResultChar = {pixelChar}");

                    Console.WriteLine();
                    Console.WriteLine("RayHierarchy:");
                    lightRay.Hierarchy.PrintHierarchy();

                    Console.WriteLine();
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        //DEBUG
        private int GetDistanceBetweenScreenPositions(Vector2Int position1, Vector2Int position2)
        {
            return (int)(Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2)));
        }

        //DEBUG
        private class RayDebugData
        {
            public readonly Vector3 startRayDirection;
            public readonly LightRay lightRay;
            public readonly float pixelColor;
            public readonly char pixelChar;
            public RayDebugData(in Vector3 startRayDirection, in LightRay lightRay, float pixelColor, char pixelChar) 
            {
                this.startRayDirection = startRayDirection;
                this.lightRay = lightRay;
                this.pixelColor = pixelColor;
                this.pixelChar = pixelChar;
            }

        }

        //DEBUG
        private void PrintDebugScreenCoordinates(int x, int y)
        {
            if (x == 0)
            {
                char pixel = char.Parse($"{(y % 10)}");

                if (y % 10 == 0)
                {
                    pixel = ' ';
                }

                screen[x + y * camera.Resolution.X] = pixel;
            }

            if (y == 0)
            {
                char pixel = char.Parse($"{(x % 10)}");

                if (x % 10 == 0)
                {
                    pixel = ' ';
                }

                screen[x + y * camera.Resolution.X] = pixel;
            }
        }

        //TODO: test sky char maybe its redundant

        private float RenderRay(LightRay ray)
        {
            FullIntersection? intersection = GetNearestIntersection(ray.Ray);

            if (ray.Intensity < MinRayIntensity 
                || (!intersection.HasValue && ray.InteractionCount > 0))
            {
                return ray.Intensity * (GetDirectionsLightIntensity(ray.Ray.Direction) + LocalScene.GlobalLight.Intensity);
            }

            if (intersection.HasValue)
            {
                ray.Interact();

                float absorptionCoefficient = GetAbsorptionCoefficient(ray.EnvironmentMaterial, intersection.Value.ShapeIntersection.MinIntersectionDistance);
                ray.Intensity /= absorptionCoefficient;

                LightRay reflectedRay = null;
                LightRay refractedRay = null;

                if (intersection.Value.Intersectable is ILight light)
                {
                    return ray.Intensity * (light.Intensity + GetDirectionsLightIntensity(ray.Ray.Direction) + LocalScene.GlobalLight.Intensity);
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

            return 0;
        }

        private float GetAbsorptionCoefficient(Material environmentMaterial, float distance)
        {
            return (float)Math.Pow(Math.E, environmentMaterial.AbsorptionRate * distance);
        }

        private float GetDirectionsLightIntensity(Vector3 rayDirection)
        {
            float directionLightIntensity = 0;

            foreach (IDirectionLight light in LocalScene.DirectionLights)
            {
                directionLightIntensity += 
                    MathExtension.Clamp(
                        Vector3.Dot(-rayDirection, light.WorldDirection) * light.Intensity,
                        0, float.MaxValue);
            }

            return directionLightIntensity;
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
