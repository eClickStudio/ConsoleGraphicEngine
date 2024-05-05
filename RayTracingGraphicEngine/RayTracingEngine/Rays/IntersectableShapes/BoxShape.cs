using RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using Engine3D.Components.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathExtensions;

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

        private float SafeDivide(float a, float b)
        {
            if (b == 0)
            {
                return 0;
            }
            else
            {
                return a / b;
            }
        }

        public override ShapeIntersection? GetShapeIntersection(Ray ray)
        {
            Vector3 position = transform.Position;
            ray.Origin = ray.Origin -= position;

            Vector3 m = new Vector3(SafeDivide(1, ray.Direction.X), SafeDivide(1, ray.Direction.Y), SafeDivide(1, ray.Direction.Z));
            Vector3 n = m * ray.Origin;
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
                //if tN or tF < 0 it means that the point of intersection lies opposite the direction of the vector
                return null;
            }

            float minDistance = distances.Min();

            Vector3 nearestIntersection = ray.Origin + ray.Direction * minDistance;

            Vector3 normalRayDirection;

            Dictionary<Vector3, Vector3> normalsByCenters = new Dictionary<Vector3, Vector3>()
            {
                { new Vector3(0, 0, Size.Z / 2), new Vector3(0, 0, 1)},
                { new Vector3(0, 0, -Size.Z / 2), new Vector3(0, 0, -1)},
                { new Vector3(Size.X / 2, 0, 0), new Vector3(1, 0, 0)},
                { new Vector3(-Size.X / 2, 0, 0), new Vector3(-1, 0, 0)},
                { new Vector3(0, Size.Y / 2, 0), new Vector3(0, 1, 0)},
                { new Vector3(0, -Size.Y / 2, 0), new Vector3(0, -1, 0)}
            };

            minDistance = float.MaxValue;
            List<Vector3> admissibleNormals = new List<Vector3>();
            foreach (KeyValuePair<Vector3, Vector3> pair in normalsByCenters)
            {
                float distance = Vector3.Distance(nearestIntersection, pair.Key);

                if (distance == minDistance)
                {
                    admissibleNormals.Add(pair.Value);
                }

                if (distance < minDistance)
                {
                    admissibleNormals.Clear();
                    admissibleNormals.Add(pair.Value);
                    minDistance = distance;
                }
            }

            Vector3 normalSum = Vector3.Zero;
            foreach (Vector3 normal in admissibleNormals)
            {
                normalSum += normal;
            }
            normalRayDirection = normalSum / admissibleNormals.Count;

            if (tN <= 0)
            {
                normalRayDirection = -normalRayDirection;
            }

            Ray normalRay = new Ray(nearestIntersection + normalRayDirection * Configurations.Configurations.MIN_RAY_STEP, normalRayDirection);

            return new ShapeIntersection(minDistance, true, normalRay);
        }

        //public override ShapeIntersection? GetShapeIntersection(Ray ray)
        //{
        //    Vector3 position = transform.Position;
        //    ray.Origin = ray.Origin -= position;

        //    Vector3 m = new Vector3(1f / ray.Direction.X, 1f / ray.Direction.Y, 1f / ray.Direction.Z);
        //    Vector3 n = m * ray.Origin;
        //    Vector3 k = Vector3.Abs(m) * Size;
        //    Vector3 t1 = -n - k;
        //    Vector3 t2 = -n + k;
        //    float tN = Math.Max(Math.Max(t1.X, t1.Y), t1.Z);
        //    float tF = Math.Min(Math.Min(t2.X, t2.Y), t2.Z);

        //    if (tN > tF || tF < 0)
        //    {
        //        return null;
        //    }

        //    List<float> distances = new List<float>();

        //    if (tN > 0)
        //    {
        //        distances.Add(tN);
        //    }
        //    if (tF > 0 && tN != tF)
        //    {
        //        distances.Add(tF);
        //    }

        //    if (distances.Count == 0)
        //    {
        //        //if tN or tF < 0 it means that the point of intersection lies opposite the direction of the vector
        //        return null;
        //    }

        //    float minDistance = distances.Min();

        //    Vector3 nearestIntersection = ray.Origin + ray.Direction * minDistance;

        //    Vector3 normalRayDirection;

        //    if (tN > 0)
        //    {
        //        //Ray origin outside box
        //        normalRayDirection = VectorExtension.Step(Vector3.One * tN, t1);
        //    }
        //    else
        //    {
        //        //Ray origin inside box
        //        normalRayDirection = VectorExtension.Step(t2, Vector3.One * tF);
        //    }

        //    //Vector3 yzx = new Vector3(t1.Y, t1.Z, t1.X);
        //    //Vector3 zxy = new Vector3(t1.Z, t1.X, t1.Y);
        //    //normalRayDirection = -ray.Direction.Sign() * t1.Step(yzx) * t1.Step(zxy);

        //    normalRayDirection *= (-VectorExtension.Sign(ray.Direction));
        //    normalRayDirection = Vector3.Normalize(normalRayDirection);

        //    if (normalRayDirection == Vector3.Zero ||
        //        !normalRayDirection.IsNormal())
        //    {
        //        //TEST: dele this
        //        Console.WriteLine("zero man");
        //        throw new ArithmeticException("Normal direction is incorrect");
        //        //Console.WriteLine(-ray.Direction.Sign());
        //        //Console.WriteLine(t1.Step(yzx));
        //        //Console.WriteLine(t1.Step(zxy));
        //    }

        //    Ray normalRay = new Ray(nearestIntersection + normalRayDirection * Configurations.Configurations.MIN_RAY_STEP, normalRayDirection);

        //    return new ShapeIntersection(minDistance, true, normalRay);
        //}
    }
}
