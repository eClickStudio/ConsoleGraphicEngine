using ConsoleGraphicEngine.Engine.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Tools;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Camera;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;
using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.RayTracingEngine
{
    internal class RayTracingGraphicEngine : AbstractGraphicEngine<RayTracingCamera, ObjectRenderer>
    {
        public uint rayIterations;

        public RayTracingGraphicEngine(uint rayIterations, uint fps) : base(fps)
        {
            this.rayIterations = rayIterations;
        }


        public override void RenderFrame()
        {
            for (int x = 0; x < camera.resolution.X; x++)
            {
                for (int y = 0; y < camera.resolution.Y; y++)
                {
                    Vector2Int screenPosition = new Vector2Int(x, y);

                    Ray ray = camera.GetRay(screenPosition);

                    char pixelChar = camera.GetChar(RenderRay(ray));

                    screen[x + y * camera.resolution.X] = pixelChar;
                }
            }

            Console.Write(screen);
        }

        private float? RenderRay(Ray ray)
        {
            float? brightness = light.intensivety;

            bool isIntersect = false;

            for (int i = 0; i < rayIterations; i++)
            {
                Intersection? nearestIntersection = GetNearestIntersection(ray);
                if (nearestIntersection.HasValue)
                {
                    isIntersect = true;

                    IObjectRenderer renderer = nearestIntersection.Value.intersectedRenderer;

                    Ray? normal = renderer.GetNormal(ray);

                    if (normal.HasValue)
                    {
                        ray = Ray.Reflect(ray, normal.Value);

                        brightness *= renderer.GetBrightness(normal.Value.direction, light.direction);
                    }
                }
                else
                {
                    if (!isIntersect)
                    {
                        brightness = null;
                    }

                    break;
                }
            }

            return brightness;
        }

        private Intersection? GetNearestIntersection(Ray ray)
        {
            bool isIntersect = false;

            float minIntersectionDistance = float.MaxValue;
            IObjectRenderer intersectedRenderer = null;

            foreach (ObjectRenderer renderer in scene.renderers)
            {
                IReadOnlyList<float> distances = renderer.GetIntersectionDistances(ray);

                if (distances != null && distances.Count > 0)
                {
                    isIntersect = true;
                    intersectedRenderer = renderer;

                    foreach (float intersectionDistance in distances)
                    {
                        if (intersectionDistance < 0)
                        {
                            throw new Exception($"Intersection distance cannot be less than 0; intersectionDistance = {intersectionDistance}");
                        }

                        if (intersectionDistance < minIntersectionDistance)
                        {
                            minIntersectionDistance = intersectionDistance;
                        }
                    }
                }
            }

            if (isIntersect)
            {
                return new Intersection(intersectedRenderer, minIntersectionDistance);
            }

            return null;
        }
    }
}
