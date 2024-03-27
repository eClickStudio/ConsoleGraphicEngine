using System.Numerics;
using System;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using MathExtensions;
using Hierarchy;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering
{
    public class LightRay : IHierarchyMember<LightRay>
    {
        private float _intensity;

        /// <summary>
        /// Intensity of light;
        /// min - 0;
        /// max - float.MaxValue;
        /// </summary>
        public float Intensity
        {
            get => _intensity;
            set
            {
                if (!value.IsNormal())
                {
                    throw new ArgumentException("LightRay intensity shold be normal (not NaN or Infinity e.t.c)");
                }

                _intensity = MathExtension.Clamp(value, 0, float.MaxValue);
            }
        }

        public Ray Ray { get; }

        /// <summary>
        /// How many times ray has been reflected or refracted;
        /// </summary>
        public uint InteractionCount { get; private set; }

        public Material EnvironmentMaterial { get; }

        //TEST: debug feature; delete when release-------------------------------------------------
        public string _rayName;
        public string HierarchyName
        {
            get
            {
                return $"{_rayName}) {InteractedShapeName} <=> {Intensity}";
            }
            set
            {
                _rayName = value;
            }
        }

        public string InteractedShapeName { get; }

        private HierarchyManager<LightRay> _trace;

        /// <summary>
        /// Trace of this lightRay's children
        /// </summary>
        public IHierarchyManager<LightRay> Hierarchy => _trace;
        //TEST: end debug features-----------------------------------------------------------------


        public LightRay(Ray ray, float intensity, Material environmentMaterial, uint interactionCount, string rayName, in LightRay parentRay, string interactedShapeName)
        {
            Ray = ray;
            Intensity = intensity;
            EnvironmentMaterial = environmentMaterial;
            InteractionCount = interactionCount;

            HierarchyName = rayName;
            InteractedShapeName = interactedShapeName;
            _trace = new HierarchyManager<LightRay>(this, parentRay);
        }

        public LightRay(Vector3 origin, Vector3 direction, float intensity, Material environmentMaterial, uint interactionCount, string rayName, in LightRay parentRay, string interactedShapeName)
            : this(new Ray(origin, direction), intensity, environmentMaterial, interactionCount, rayName, parentRay, interactedShapeName) { }

        public void Interact()
        {
            InteractionCount++;
        }

        public void AddHierarchyChildren(in LightRay reflectedRay, in LightRay refractedRay)
        {
            if (reflectedRay != null)
            {
                _trace.AddChild(reflectedRay);
            }

            if (refractedRay != null)
            {
                _trace.AddChild(refractedRay);
            }
        }
    }
}
