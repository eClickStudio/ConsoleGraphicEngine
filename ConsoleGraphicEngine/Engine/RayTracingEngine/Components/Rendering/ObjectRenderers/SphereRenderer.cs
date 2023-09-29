using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers
{
    public class SphereRenderer : ObjectRenderer
    {
        public float Radius { get; }

        public SphereRenderer(Material material, float radius) : base(material)
        {
            this.Radius = radius;
        }

        public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        {
            ray.Origin -= ParentObject.ThisTransform.Position;
            float difference = Vector3.Dot(ray.Origin, ray.Direction);
            Vector3 qc = ray.Origin - difference * ray.Direction;
            float h = Radius * Radius - Vector3.Dot(qc, qc);

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

            Vector3 position = ParentObject.ThisTransform.Position;
            Vector3 direction = Vector3.Normalize(nearestIntersection.Value - position);


            if (nearestIntersection.Value == position)
            {
                throw new ArgumentException("Incorrect position");
            }

            return new Ray(nearestIntersection.Value + direction * _MIN_RAY_STEP, direction);
        }
    }
}
