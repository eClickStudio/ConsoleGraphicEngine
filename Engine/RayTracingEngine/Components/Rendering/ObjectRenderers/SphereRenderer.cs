using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers
{
    class SphereRenderer : ObjectRenderer
    {
        public float radius { get; }

        public SphereRenderer(Material material, float radius) : base(material)
        {
            this.radius = radius;
        }

        public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        {
            ray.origin -= parentObject.transform.position;
            float difference = Vector3.Dot(ray.origin, ray.direction);
            Vector3 qc = ray.origin - difference * ray.direction;
            float h = radius * radius - Vector3.Dot(qc, qc);

            if (h < 0)
            {
                return null;
            }
            else if (h == 0)
            {
                return new List<float>() { -difference };
            }

            h = (float)Math.Sqrt(h);

            List<float> distances = new List<float>();
            if (-difference - h >= 0)
            {
                distances.Add(-difference - h);
            }
            if (-difference + h >= 0)
            {
                distances.Add(-difference + h);
            }

            return distances.Count > 0 ? distances : null;

            //Vector3 position = parentObject.transform.position;

            //float distanceToSphere = Vector3.Distance(ray.startPosition, position);

            //Vector3 directionRayStartToShphere = Vector3.Normalize(position - ray.startPosition);
            //float angleCos = Vector3.Dot(ray.direction, directionRayStartToShphere);

            //float distanceRayStartToNearest = angleCos * distanceToSphere;
            //float minDistance = (float)Math.Sqrt(Math.Round(distanceToSphere * distanceToSphere - distanceRayStartToNearest * distanceRayStartToNearest, 3));

            //if (minDistance > radius)
            //{
            //    return null;
            //}

            //float centerOffset = (float)Math.Sqrt(radius * radius - minDistance * minDistance);
            //float distanceTo1Point = distanceRayStartToNearest - centerOffset;
            //float distanceTo2Point = distanceRayStartToNearest + centerOffset;

            //Lazy<List<float>> intersections = new Lazy<List<float>>();

            //if (distanceTo1Point >= 0)
            //{
            //    intersections.Value.Add(distanceTo1Point);
            //}
            //if (distanceTo2Point >= 0 && distanceTo1Point != distanceTo2Point)
            //{
            //    intersections.Value.Add(distanceTo2Point);
            //}

            //if (intersections.IsValueCreated)
            //{
            //    return intersections.Value;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public override Ray? GetNormal(Ray ray)
        {
            Vector3? nearestIntersection = GetNearestIntersection(ray);

            if (!nearestIntersection.HasValue)
            {
                return null;
            }

            Vector3 position = parentObject.transform.position;
            Vector3 direction = Vector3.Normalize(nearestIntersection.Value - position);


            if (nearestIntersection.Value == position)
            {
                throw new ArgumentException("Incorrect position");
            }

            return new Ray(nearestIntersection.Value + direction * _RAY_STEP, direction);
        }
    }
}
