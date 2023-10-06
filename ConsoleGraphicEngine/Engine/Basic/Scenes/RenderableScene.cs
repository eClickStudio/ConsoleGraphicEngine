using ConsoleGraphicEngine3D.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Light;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using Engine3D.Objects;
using Engine3D.Scenes;
using System.Collections.Generic;

namespace ConsoleGraphicEngine3D.Engine.Basic.Scenes
{
    public class RenderableScene<CameraType, RendererType> : Scene, IRenderableScene<CameraType, RendererType>
        where CameraType : class, ICamera
        where RendererType : class, IRenderer
    {
        public CameraType MainCamera { get; set; }

        public IDirectionLight GlobalLight { get; set; }


        protected List<RendererType> ObjectRenderers { get; }
        public IReadOnlyList<RendererType> Renderers => ObjectRenderers;

        public RenderableScene()
        {
            ObjectRenderers = new List<RendererType>();
        }

        public override void AddObject(in IObject3D object3D)
        {
            if (object3D != null && !ContainObject(object3D))
            {
                if (object3D.TryGetComponent(out RendererType renderer))
                {
                    ObjectRenderers.Add(renderer);
                }
            }

            base.AddObject(object3D);
        }

        public override void RemoveObject(in IObject3D object3D)
        {
            if (object3D != null && ContainObject(object3D))
            {
                if (object3D.TryGetComponent(out RendererType renderer))
                {
                    ObjectRenderers.Remove(renderer);
                }
            }

            base.RemoveObject(object3D);
        }
    }
}
