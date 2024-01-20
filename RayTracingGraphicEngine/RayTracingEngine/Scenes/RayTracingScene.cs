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

        private IDirectionLight _globalLight;
        public IDirectionLight GlobalLight
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


        protected readonly List<IIntersectable> _intersectables;
        public IReadOnlyList<IIntersectable> Intersectables => _intersectables;

        public RayTracingScene(Material environmentMaterial)
        {
            EnvironmentMaterial = environmentMaterial;
            _intersectables = new List<IIntersectable>();
        }

        private IReadOnlyList<IComponent> FindIntersectableComponents(in IObject3D object3D)
        {
            Predicate<IComponent> match = (component) => component is IIntersectable;
            return object3D.FindAllComponents(match);
        }

        public override void AddObject(in IObject3D object3D)
        {
            if (object3D != null && !ContainObject(object3D))
            {
                IReadOnlyList<IComponent> intersectableComponents = FindIntersectableComponents(object3D);

                if (intersectableComponents.Count > 0)
                {
                    foreach (IComponent component in intersectableComponents)
                    {
                        if (component is IIntersectable intersectable)
                        {
                            _intersectables.Add(intersectable);
                        }
                    }
                }
            }

            base.AddObject(object3D);
        }

        public override void RemoveObject(in IObject3D object3D)
        {
            if (object3D != null && ContainObject(object3D))
            {
                IReadOnlyList<IComponent> intersectableComponents = FindIntersectableComponents(object3D);

                if (intersectableComponents.Count > 0)
                {
                    foreach (IComponent component in intersectableComponents)
                    {
                        if (component is IIntersectable intersectable)
                        {
                            _intersectables.Remove(intersectable);
                        }
                    }
                }
            }

            base.RemoveObject(object3D);
        }
    }
}
