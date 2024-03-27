using Engine3D.Objects;
using Engine3D.Scenes;
using System.Collections.Generic;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.Abstract.Scenes;
using System;
using Engine3D.Components.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering;
using System.Text.RegularExpressions;
using System.Linq;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Scenes
{
    //TODO: fix ObjectRenderers must be list of intersectables
    public class RayTracingScene : Scene, IRenderableScene<IRayTracingCamera>
    {
        private Material _environmentMaterial;

        /// <summary>
        /// Outside material
        /// </summary>
        public Material EnvironmentMaterial 
        {
            get => _environmentMaterial;
            set
            {
                if (value != _environmentMaterial)
                {
                    _environmentMaterial = value;

                    if (MainCamera != null)
                    {
                        MainCamera.SceneEnvironmentMaterial = _environmentMaterial;
                    }

                    OnChanged();
                }
            }
        }

        private IRayTracingCamera _mainCamera;
        public IRayTracingCamera MainCamera
        {
            get => _mainCamera;
            set
            {
                if (_mainCamera != value)
                {
                    _mainCamera = value;
                    _mainCamera.SceneEnvironmentMaterial = EnvironmentMaterial;

                    OnChanged();
                }
            }
        }

        private ILight _globalLight;
        public ILight GlobalLight
        {
            get => _globalLight;
            set
            {
                if (_globalLight != value)
                {
                    _globalLight = value;

                    OnChanged();
                }
            }
        }

        private readonly Predicate<IComponent> _intersectableMatchPredicate = (component) => component is IIntersectable;
        protected readonly List<IIntersectable> _intersectables;
        public IReadOnlyList<IIntersectable> Intersectables => _intersectables;


        private readonly Predicate<IComponent> _directionLightMatchPredicate = (component) => component is IDirectionLight;
        protected readonly List<IDirectionLight> _directionLights; 
        public IList<IDirectionLight> DirectionLights => _directionLights;

        public RayTracingScene(Material environmentMaterial)
        {
            EnvironmentMaterial = environmentMaterial;
            _intersectables = new List<IIntersectable>();
            _directionLights = new List<IDirectionLight>();
        }

        private void RegisterObjectComponents<T>(List<T> registrationList, in IObject3D object3D, Predicate<IComponent> matchPredicate)
        {
            IReadOnlyList<IComponent> foundComponents = object3D.FindAllComponents(matchPredicate);

            if (foundComponents.Count > 0)
            {
                foreach (T component in foundComponents.Select(i => (T)i))
                {
                    registrationList.Add(component);
                }
            }
        }

        private void UnregisterObjectComponents<T>(List<T> registrationList, in IObject3D object3D, Predicate<IComponent> matchPredicate)
        {
            IReadOnlyList<IComponent> foundComponents = object3D.FindAllComponents(matchPredicate);

            if (foundComponents.Count > 0)
            {
                foreach (T component in foundComponents.Select(i => (T)i))
                {
                    registrationList.Remove(component);
                }
            }
        }

        public override void AddObject(in IObject3D object3D)
        {
            if (object3D != null && !ContainObject(object3D))
            {
                RegisterObjectComponents(_intersectables, object3D, _intersectableMatchPredicate);
                RegisterObjectComponents(_directionLights, object3D, _directionLightMatchPredicate);
            }

            base.AddObject(object3D);
        }

        public override void RemoveObject(in IObject3D object3D)
        {
            if (object3D != null && ContainObject(object3D))
            {
                UnregisterObjectComponents(_intersectables, object3D, _intersectableMatchPredicate);
                UnregisterObjectComponents(_directionLights, object3D, _directionLightMatchPredicate);
            }

            base.RemoveObject(object3D);
        }
    }
}
