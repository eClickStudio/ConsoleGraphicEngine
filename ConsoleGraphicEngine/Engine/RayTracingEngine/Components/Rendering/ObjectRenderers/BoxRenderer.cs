using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers
{
    public class BoxRenderer : ObjectRenderer
    {
        //TODO: using rotation to intersect

        public Vector3 Size { get; }

        public BoxRenderer(Material material, Vector3 size) : base(material)
        {
            Size = size;
        }

        public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        {
            Vector3 position = ParentObject.ThisTransform.Position;
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

            List<float> intersectionDistances = new List<float>();

            if (tN > 0)
            {
                intersectionDistances.Add(tN);
            }
            if (tF > 0 && tN != tF)
            {
                intersectionDistances.Add(tF);
            }

            return intersectionDistances;
        }

        public override Ray? GetNormal(Ray ray)
        {
            Vector3? nearestIntersection = GetNearestIntersection(ray);

            if (!nearestIntersection.HasValue)
            {
                return null;
            }

            Vector3 position = ParentObject.ThisTransform.Position;
            ray.Origin -= position;

            Vector3 m = new Vector3(1f / ray.Direction.X, 1f / ray.Direction.Y, 1f / ray.Direction.Z); // can precompute if traversing a set of aligned boxes
            Vector3 n = m * ray.Origin;   // can precompute if traversing a set of aligned boxes
            Vector3 k = Vector3.Abs(m) * Size;
            Vector3 t1 = -n - k;

            Vector3 direction;

            Vector3 yzx = new Vector3(t1.Y, t1.Z, t1.X);
            Vector3 zxy = new Vector3(t1.Z, t1.X, t1.Y);
            direction = -Vector3Math.Sign(ray.Direction) * Vector3Math.Step(yzx, t1) * Vector3Math.Step(zxy, t1);

            return new Ray(nearestIntersection.Value + direction * _MIN_RAY_STEP, direction);
        }
    }
}
