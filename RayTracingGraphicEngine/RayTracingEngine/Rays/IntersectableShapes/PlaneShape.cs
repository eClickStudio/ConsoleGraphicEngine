using RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays.Intersections;
using Engine3D.Components.Transform;
using System;
using System.Numerics;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Configurations;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes
{
    public class PlaneShape : AbstractIntersectableShape
    {
        private Vector3 _normalVector;
        
        /// <summary>
        /// Normal vector to this plane
        /// </summary>
        public Vector3 NormalVector 
        {
            get => _normalVector;
            set
            {
                if (!value.IsNormal() || value == Vector3.Zero)
                {
                    throw new ArgumentException($"Normal vector you want to set is invalid; Value = {value}");
                }

                value = Vector3.Normalize(value);

                if (_normalVector != value)
                {
                    _normalVector = value;

                    OnChanged();
                }
            }
        }


        private float _offset;

        /// <summary>
        /// Offset of plane
        /// </summary>
        public float Offset
        {
            get => _offset;
            set 
            { 
                if (!value.IsNormal())
                {
                    throw new ArgumentException($"Offset you want to set is invalid; Value = {value}");
                }

                if (_offset != value)
                {
                    _offset = value;

                    OnChanged();
                }
            }
        }


        public PlaneShape(in ITransform transform, Vector3 normalVector, float offset) : base(transform)
        {
            NormalVector = normalVector;
            Offset = offset;
        }

        public override ShapeIntersection? GetShapeIntersection(Ray ray)
        {
            float distance = -(Vector3.Dot(ray.Origin, NormalVector) + Offset) / Vector3.Dot(ray.Direction, NormalVector);
            
            if (distance < 0)
            {
                return null;
            }

            Ray normalRay = new Ray(ray.Origin + ray.Direction * distance + NormalVector * Configurations.Configurations.MIN_RAY_STEP, NormalVector);
            return new ShapeIntersection(distance, false, normalRay);
        }
    }
}
