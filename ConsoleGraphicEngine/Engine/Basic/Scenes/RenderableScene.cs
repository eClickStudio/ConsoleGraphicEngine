using ConsoleGraphicEngine3D.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Light;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using Engine3D.Components.Transform;
using Engine3D.Objects;
using Engine3D.Scenes;
using System;
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
            if (object3D == null)
            {
                throw new ArgumentException("You can not add null object");
            }

            if (!ContainObject(object3D))
            {
                AttachedObjects.Add(object3D);

                if (object3D is RendererType renderer)
                {
                    ObjectRenderers.Add(renderer);
                }

                ITransform transform = object3D.ThisTransform;

                if (transform.ParentObject != null &&
                    !ContainObject(transform.ParentObject))
                {
                    transform.ParentObject = null;
                }

                foreach (ITransform child in transform.Hierarchy.Children)
                {
                    AddObject(child.ParentObject);
                }

                OnChanged();
            }
        }

        public override void RemoveObject(in IObject3D object3D)
        {
            if (object3D == null)
            {
                throw new ArgumentException("You can not remove null object");
            }

            if (ContainObject(object3D))
            {
                AttachedObjects.Remove(object3D);

                if (object3D is RendererType renderer)
                {
                    ObjectRenderers.Remove(renderer);
                }

                foreach (ITransform child in object3D.ThisTransform.Hierarchy.Children)
                {
                    RemoveObject(child.ParentObject);
                }

                OnChanged();
            }
        }
    }
}
