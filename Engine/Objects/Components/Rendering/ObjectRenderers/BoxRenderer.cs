using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers.Abstract;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers
{
    internal class BoxRenderer : ObjectRenderer
    {
        public Vector3 size { get; }

        public BoxRenderer(Material material, Vector3 size) : base(material)
        {
            this.size = size;
        }

        public override IReadOnlyList<float> GetIntersectionDistances(Ray ray)
        {
            Vector3 position = parentObject.transform.position;
            ray.origin = ray.origin -= position;

            Vector3 m = new Vector3(1f / ray.direction.X, 1f / ray.direction.Y, 1f / ray.direction.Z); // can precompute if traversing a set of aligned boxes
            Vector3 n = m * ray.origin;   // can precompute if traversing a set of aligned boxes
            Vector3 k = Vector3.Abs(m) * size;
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

            Vector3 position = parentObject.transform.position;
            ray.origin -= position;

            Vector3 m = new Vector3(1f / ray.direction.X, 1f / ray.direction.Y, 1f / ray.direction.Z); // can precompute if traversing a set of aligned boxes
            Vector3 n = m * ray.origin;   // can precompute if traversing a set of aligned boxes
            Vector3 k = Vector3.Abs(m) * size;
            Vector3 t1 = -n - k;

            Vector3 direction;

            Vector3 yzx = new Vector3(t1.Y, t1.Z, t1.X);
            Vector3 zxy = new Vector3(t1.Z, t1.X, t1.Y);
            direction = -Vector3Math.Sign(ray.direction) * Vector3Math.Step(yzx, t1) * Vector3Math.Step(zxy, t1);

            return new Ray(nearestIntersection.Value + direction * _RAY_STEP, direction);
        }
    }
}
