using RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using Engine3D.Components.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Configurations;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes
{
    public class BoxShape : AbstractIntersectableShape
    {
        //TODO: using rotation to intersect

        private Vector3 _size;

        /// <summary>
        /// Size of box
        /// </summary>
        public Vector3 Size 
        {
            get => _size;
            set
            {
                if (!value.IsNormal() || value.IsSomeValueEqual(0) 
                    || Vector3.Clamp(value, Vector3.Zero, Vector3.One * float.MaxValue) != value)
                {
                    throw new ArgumentException($"Box size you want to set is invalid; Value = {value}");
                }

                if (_size != value)
                {
                    _size = value;

                    OnChanged();
                }
            } 
        }

        public BoxShape(in ITransform transform, Vector3 size) : base(transform)
        {
            Size = size;
        }

        public override ShapeIntersection? GetShapeIntersection(Ray ray)
        {
            Vector3 position = transform.Position;
            ray.Origin = ray.Origin -= position;

            Vector3 m = new Vector3(1f / ray.Direction.X, 1f / ray.Direction.Y, 1f / ray.Direction.Z); // can precompute if traversing a set of aligned boxes
            Vector3 n = m * ray.Origin;   // can precompute if traversing a set of aligned boxes
            Vector3 k = Vector3.Abs(m) * Size;
            Vector3 t1 = -n - k;
            Vector3 t2 = -n + k;
            float tN = Math.Max(Math.Max(t1.X, t1.Y), t1.Z);
            float tF = Math.Min(Math.Min(t2.X, t2.Y), t2.Z);

            if (tN > tF || tF < 0)
            {
                return null;
            }

            List<float> distances = new List<float>();

            if (tN > 0)
            {
                distances.Add(tN);
            }
            if (tF > 0 && tN != tF)
            {
                distances.Add(tF);
            }

            if (distances.Count == 0)
            {
                //TODO: TEST: May be its a crutch
                return null;
            }

            float minDistance = distances.Min();

            Vector3 nearestIntersection = ray.Origin + ray.Direction * minDistance;

            Vector3 direction;

            Vector3 yzx = new Vector3(t1.Y, t1.Z, t1.X);
            Vector3 zxy = new Vector3(t1.Z, t1.X, t1.Y);
            direction = -ray.Direction.Sign() * t1.Step(yzx) * t1.Step(zxy);

            if (direction == Vector3.Zero)
            {
                Console.WriteLine(-ray.Direction.Sign());
                Console.WriteLine(t1.Step(yzx));
                Console.WriteLine(t1.Step(zxy));
            }

            Ray normalRay = new Ray(nearestIntersection + direction * Configurations.Configurations.MIN_RAY_STEP, direction);

            //TODO: didPassThroughtEnvironment should only be true if the ray is propagating through the inside of the shape.
            return new ShapeIntersection(minDistance, true, normalRay);
        }

        //public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        //{
        //    Vector3 position = transform.Position;
        //    ray.Origin = ray.Origin -= position;

        //    Vector3 m = new Vector3(1f / ray.Direction.X, 1f / ray.Direction.Y, 1f / ray.Direction.Z); // can precompute if traversing a set of aligned boxes
        //    Vector3 n = m * ray.Origin;   // can precompute if traversing a set of aligned boxes
        //    Vector3 k = Vector3.Abs(m) * Size;
        //    Vector3 t1 = -n - k;
        //    Vector3 t2 = -n + k;
        //    float tN = Math.Max(Math.Max(t1.X, t1.Y), t1.Z);
        //    float tF = Math.Min(Math.Min(t2.X, t2.Y), t2.Z);

        //    if (tN > tF || tF < 0)
        //    {
        //        return null;
        //    }

        //    List<float> intersectionDistances = new List<float>();

        //    if (tN > 0)
        //    {
        //        intersectionDistances.Add(tN);
        //    }
        //    if (tF > 0 && tN != tF)
        //    {
        //        intersectionDistances.Add(tF);
        //    }

        //    return intersectionDistances;
        //}

        //public override Ray GetNormal(Ray ray)
        //{
        //    Vector3? nearestIntersection = GetNearestIntersection(ray);

        //    if (!nearestIntersection.HasValue)
        //    {
        //        return null;
        //    }

        //    Vector3 position = transform.Position;
        //    ray.Origin -= position;

        //    Vector3 m = new Vector3(1f / ray.Direction.X, 1f / ray.Direction.Y, 1f / ray.Direction.Z); // can precompute if traversing a set of aligned boxes
        //    Vector3 n = m * ray.Origin;   // can precompute if traversing a set of aligned boxes
        //    Vector3 k = Vector3.Abs(m) * Size;
        //    Vector3 t1 = -n - k;

        //    Vector3 direction;

        //    Vector3 yzx = new Vector3(t1.Y, t1.Z, t1.X);
        //    Vector3 zxy = new Vector3(t1.Z, t1.X, t1.Y);
        //    direction = -Vector3Math.Sign(ray.Direction) * Vector3Math.Step(yzx, t1) * Vector3Math.Step(zxy, t1);

        //    return new Ray(nearestIntersection.Value + direction * MIN_RAY_STEP, direction);
        //}
    }
}
