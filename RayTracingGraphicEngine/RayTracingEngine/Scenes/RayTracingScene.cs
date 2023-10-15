using Engine3D.Objects;
using Engine3D.Scenes;
using System.Collections.Generic;
using RayTracingGraphicEngine3D.Rays;
using RayTracingGraphicEngine3D.Components.Intersectable;
using RayTracingGraphicEngine3D.Components.Light.Abstract;
using RayTracingGraphicEngine3D.Components.Camera.Abstract;
using RayTracingGraphicEngine3D.Abstract.Scenes;

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

        public override void AddObject(in IObject3D object3D)
        {
            if (object3D != null && !ContainObject(object3D))
            {
                if (object3D.TryGetComponent(out IIntersectableComponent renderer))
                {
                    System.Console.WriteLine("Add intersectable component");
                    _intersectables.Add(renderer);
                }
            }

            base.AddObject(object3D);
        }

        public override void RemoveObject(in IObject3D object3D)
        {
            if (object3D != null && ContainObject(object3D))
            {
                if (object3D.TryGetComponent(out IIntersectableComponent renderer))
                {
                    _intersectables.Remove(renderer);
                }
            }

            base.RemoveObject(object3D);
        }
    }
}
