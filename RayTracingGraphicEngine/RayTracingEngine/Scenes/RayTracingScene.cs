using Engine3D.Objects;
using Engine3D.Scenes;
using System.Collections.Generic;
using RayTracingGraphicEngine3D.Rays;
using RayTracingGraphicEngine3D.Components.Light.Abstract;
using RayTracingGraphicEngine3D.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.Abstract.Scenes;
using System;
using Engine3D.Components.Abstract;

namespace RayTracingGraphicEngine3D.Scenes
{
    //TODO: fix ObjectRenderers must be list of intersectables
    public class RayTracingScene : Scene, IRenderableScene<IRayTracingCamera>
    {
        public IRayTracingCamera MainCamera { get; set; }

        public IDirectionLight GlobalLight { get; set; }


        protected readonly List<IIntersectable> _intersectables;
        public IReadOnlyList<IIntersectable> Intersectables => _intersectables;

        public RayTracingScene()
        {
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
