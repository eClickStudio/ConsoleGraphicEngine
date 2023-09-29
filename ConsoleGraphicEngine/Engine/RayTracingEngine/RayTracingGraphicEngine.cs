using ConsoleGraphicEngine3D.Engine.Abstract;
using ConsoleGraphicEngine3D.Engine.Basic.Tools;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Camera;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;
using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine
{
    public class RayTracingGraphicEngine : AbstractGraphicEngine<RayTracingCamera, ObjectRenderer>
    {
        public uint rayIterations;

        public RayTracingGraphicEngine(uint rayIterations, uint fps) : base(fps)
        {
            this.rayIterations = rayIterations;
        }


        public override void RenderFrame()
        {
            for (int x = 0; x < camera.Resolution.X; x++)
            {
                for (int y = 0; y < camera.Resolution.Y; y++)
                {
                    Vector2Int screenPosition = new Vector2Int(x, y);

                    Ray ray = camera.GetRay(screenPosition);

                    char pixelChar = camera.GetChar(RenderRay(ray));

                    screen[x + y * camera.Resolution.X] = pixelChar;
                }
            }

            Console.Write(screen);
        }

        private float? RenderRay(Ray ray)
        {
            float? brightness = light.Intensivety;

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

                        brightness *= renderer.GetBrightness(normal.Value.Direction, light.Direction);
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

            foreach (ObjectRenderer renderer in RenderingScene.Renderers)
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
