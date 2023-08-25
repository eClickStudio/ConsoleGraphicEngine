using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers
{
    class SphereObjectRenderer : ObjectRenderer
    {
        public float radius { get; }

        public SphereObjectRenderer(float radius)
        {
            this.radius = radius;
        }

        public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        {
            Vector3 position = parentObject.transform.position;

            float distanceToSphere = Vector3.Distance(ray.startPosition, position);

            Vector3 directionRayStartToShphere = Vector3.Normalize(position - ray.startPosition);
            float angleCos = Vector3.Dot(ray.direction, directionRayStartToShphere);

            float distanceRayStartToNearest = angleCos * distanceToSphere;
            float minDistance = (float)Math.Sqrt(Math.Round(distanceToSphere * distanceToSphere - distanceRayStartToNearest * distanceRayStartToNearest, 3));

            if (minDistance > radius)
            {
                return null;
            }

            float centerOffset = (float)Math.Sqrt(radius * radius - minDistance * minDistance);
            float distanceTo1Point = distanceRayStartToNearest - centerOffset;
            float distanceTo2Point = distanceRayStartToNearest + centerOffset;

            Lazy<List<float>> intersections = new Lazy<List<float>>();

            if (distanceTo1Point >= 0)
            {
                intersections.Value.Add(distanceTo1Point);
            }
            if (distanceTo2Point >= 0 && distanceTo1Point != distanceTo2Point)
            {
                intersections.Value.Add(distanceTo2Point);
            }

            if (intersections.IsValueCreated)
            {
                return intersections.Value;
            }
            else
            {
                return null;
            }
        }

        public override Ray GetNormal(Vector3 position)
        {
            Vector3 centerPosition = parentObject.transform.position;

            return new Ray(position, Vector3.Normalize(position - centerPosition));
        }
    }
}
