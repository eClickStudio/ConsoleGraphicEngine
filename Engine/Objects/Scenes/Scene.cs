using ConsoleGraphicEngine.Engine.Objects.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.Light;
using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Objects.Scenes
{
    class Scene : IScene
    {
        private List<IObject3D> _objects { get; }
        private List<IVisibleObject> _visibleObjects { get; }
        public Camera mainCamera { get; private set; }
        public GlobalLight globalLight { get; private set; }

        public Scene()
        {
            _objects = new List<IObject3D>();
            _visibleObjects = new List<IVisibleObject>();
        }

        public void AddObject(in IObject3D object3D)
        {
            if (!ContainsObject(object3D))
            {
                _objects.Add(object3D);

                if (object3D is IVisibleObject visibleObject)
                {
                    _visibleObjects.Add(visibleObject);
                }

                if (mainCamera == null && object3D.TryGetComponent(out Camera camera))
                {
                    mainCamera = camera;
                }

                if (globalLight == null && object3D.TryGetComponent(out GlobalLight light))
                {
                    globalLight = light;
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

        public bool ContainsObject(in IObject3D object3D)
        {
            return _objects.Contains(object3D);
        }

        public void RemoveObject(in IObject3D object3D)
        {
            if (ContainsObject(object3D))
            {
                _objects.Remove(object3D);

                if (object3D is IVisibleObject visibleObject)
                {
                    _visibleObjects.Remove(visibleObject);
                }
            }
        }

        public IReadOnlyList<IObject3D> FindObjectsByName(string name)
        {
            Predicate<IObject3D> predicate = (IObject3D object3D) => object3D.name == name;

            return _objects.FindAll(predicate);
        }

        public IReadOnlyList<IObject3D> FindObjectsByComponent(Component component)
        {
            Predicate<IObject3D> predicate = (IObject3D object3D) => object3D.ContainsComponent(component);
            
            return _objects.FindAll(predicate);
        }

        public IReadOnlyList<IObject3D> GetObjects()
        {
            return _objects;
        }

        public IReadOnlyList<IVisibleObject> GetVisibleObjects()
        {
            return _visibleObjects;
        }
    }
}
