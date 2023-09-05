using ConsoleGraphicEngine.Engine.Basic.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Basic.Objects;
using System;
using System.Collections.Generic;

using ConsoleGraphicEngine.Engine.Objects

namespace ConsoleGraphicEngine.Engine.Basic.Scenes
{
    internal abstract class AbstractScene<RendererType> : IScene<RendererType>
        where RendererType : class, IRenderer
    {
        public ICamera mainCamera { get; private set; }
        public ILight globalLight { get; private set; }


        private List<IObject3D> _objects { get; }
        private List<RendererType> _renderers { get; }

        public IReadOnlyList<IObject3D> objects => _objects;

        public IReadOnlyList<RendererType> renderers => _renderers;

        public AbstractScene(ICamera camera, ILight light)
        {
            _objects = new List<IObject3D>();
            _renderers = new List<RendererType>();

            mainCamera = camera;
            globalLight = light;

            AddObjects(camera.parentObject, light.parentObject);
        }

        public bool Update()
        {
            bool didUpdate = false;

            foreach (IChangebleUpdateble someObject in _objects)
            {
                someObject.Update();

                if (someObject.CheckChanges())
                {
                    didUpdate = true;
                }
            }

            return didUpdate;
        }


        public bool ContainObject(in IObject3D object3D)
        {
            return _objects.Contains(object3D);
        }
        public bool ContainObjects(in IObject3D[] objects3D)
        {
            foreach (IObject3D object3D in objects3D)
            {
                if (!ContainObject(object3D))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainObjects(params IObject3D[] objects3D)
        {
            return ContainObjects(objects3D);
        }


        public void AddObject(in IObject3D object3D)
        {
            if (!ContainObject(object3D))
            {
                _objects.Add(object3D);

                if (object3D is RendererType renderer)
                {
                    _renderers.Add(renderer);
                }
            }
        }

        public void AddObjects(in IObject3D[] objects3D)
        {
            foreach (IObject3D object3D in objects3D)
            {
                AddObject(object3D);
            }
        }

        public void AddObjects(params IObject3D[] objects3D)
        {
            foreach (IObject3D object3D in objects3D)
            {
                AddObject(object3D);
            }
        }


        public void RemoveObject(in IObject3D object3D)
        {
            if (ContainObject(object3D))
            {
                _objects.Remove(object3D);

                if (object3D is RendererType visibleObject)
                {
                    _renderers.Remove(visibleObject);
                }
            }
        }

        public void RemoveObjects(in IObject3D[] objects3D)
        {
            foreach (IObject3D object3D in objects3D)
            {
                RemoveObject(object3D);
            }
        }

        public void RemoveObjects(params IObject3D[] objects3D)
        {
            RemoveObjects(objects3D);
        }

        public IReadOnlyList<IObject3D> FindObjectsByName(string name)
        {
            Predicate<IObject3D> predicate = (object3D) => object3D.name == name;

            return _objects.FindAll(predicate);
        }

        public IReadOnlyList<IObject3D> FindObjectsByComponent(IComponent component)
        {
            Predicate<IObject3D> predicate = (object3D) => object3D.ContainComponent(component);

            return _objects.FindAll(predicate);
        }
    }
}
