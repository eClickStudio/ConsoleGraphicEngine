using RayTracingGraphicEngine3D.Rays.IntersectableShapes.Abstract;
using RayTracingGraphicEngine3D.Rays.Intersections;
using Engine3D.Components.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Configurations;

namespace RayTracingGraphicEngine3D.Rays.IntersectableShapes
{
    public class SphereShape : AbstractIntersectableShape
    {
        //TODO: replace MIN_RAY_STEP; using one minimum value in all engine e.g. 0.001f

        private float _radius;
        public float Radius 
        {
            get => _radius;
            set
            {
                if (!value.IsNormal() || value == 0)
                {
                    throw new ArgumentException($"Radius you want to set is invalid; Value = {value}");
                }

                if (_radius != value)
                {
                    _radius = value;

                    OnChanged();
                }
            }
        }

        public SphereShape(in ITransform transform, float radius) : base(transform)
        {
            Radius = radius;
        }

        public override ShapeIntersection? GetShapeIntersection(Ray ray)
        {
            List<float> distances = new List<float>();

            ray.Origin -= transform.Position;
            float difference = Vector3.Dot(ray.Origin, ray.Direction);
            Vector3 qc = ray.Origin - difference * ray.Direction;
            float h = Radius * Radius - Vector3.Dot(qc, qc);

            if (h < 0)
            {
                return null;
            }
            else if (h == 0)
            {
                distances.Add(-difference);
            }
            else
            {
                h = (float)Math.Sqrt(h);

                if (-difference - h >= 0)
                {
                    distances.Add(-difference - h);
                }
                if (-difference + h >= 0)
                {
                    distances.Add(-difference + h);
                }

                if (distances.Count == 0)
                {
                    //May be this condition is crutch
                    return null;
                }
            }

            float minDistance = distances.Min();

            Vector3 nearestIntersection = ray.Origin + ray.Direction * minDistance;

            if (nearestIntersection == transform.Position)
            {
                throw new ArgumentException("Incorrect radius");
            }

            Vector3 direction = Vector3.Normalize(nearestIntersection - transform.Position);
            Ray NormalRay = new Ray(nearestIntersection + direction * Configurations.MIN_RAY_STEP, direction);

            //TODO: didPassThroughtEnvironment should only be true if the ray is propagating through the inside of the shape.
            return new ShapeIntersection(minDistance, true, NormalRay);
        }

        //public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        //{
        //    ray.Origin -= transform.Position;
        //    float difference = Vector3.Dot(ray.Origin, ray.Direction);
        //    Vector3 qc = ray.Origin - difference * ray.Direction;
        //    float h = Radius * Radius - Vector3.Dot(qc, qc);

        //    if (h < 0)
        //    {
        //        return null;
        //    }
        //    else if (h == 0)
        //    {
        //        return new List<float>() { -difference };
        //    }

        //    h = (float)Math.Sqrt(h);

        //    List<float> distances = new List<float>();
        //    if (-difference - h >= 0)
        //    {
        //        distances.Add(-difference - h);
        //    }
        //    if (-difference + h >= 0)
        //    {
        //        distances.Add(-difference + h);
        //    }

        //    return distances.Count > 0 ? distances : null;
        //}

        //public override Ray GetNormal(Ray ray)
        //{
        //    Vector3? nearestIntersection = GetNearestIntersection(ray);

        //    if (!nearestIntersection.HasValue)
        //    {
        //        return null;
        //    }

        //    Vector3 position = transform.Position;
        //    Vector3 direction = Vector3.Normalize(nearestIntersection.Value - position);


        //    if (nearestIntersection.Value == position)
        //    {
        //        throw new ArgumentException("Incorrect position");
        //    }

        //    return new Ray(nearestIntersection.Value + direction * MIN_RAY_STEP, direction);
        //}
    }
}
