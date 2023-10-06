using Engine3D.ChangeTriggers;
using Engine3D.Components.Abstract;
using Engine3D.Components.Transform;
using Engine3D.Objects;
using Hierarchy;
using System;
using System.Collections.Generic;

namespace Engine3D.Scenes
{
    public class Scene : AbstractChangebleUpdateble, IScene
    {
        //TODO: test scene transform. If sceneTransform changes triggers onChanged

        public ITransform SceneTransform { get; }

        protected List<IObject3D> AttachedObjects { get; }

        public IReadOnlyList<IObject3D> Objects => AttachedObjects;


        public Scene()
        {
            AttachedObjects = new List<IObject3D>();

            ChangableUpdatebleChildren = AttachedObjects;

            SceneTransform = new Transform(null);
            SceneTransform.HierarchyName = "Scene";
        }


        public virtual bool ContainObject(in IObject3D object3D)
        {
            return AttachedObjects.Contains(object3D);
        }

        private bool ContainObjectsArray(in IObject3D[] objects3D)
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

        public bool ContainObjects(in IObject3D[] objects3D)
        {
            return ContainObjectsArray(objects3D);
        }

        public bool ContainObjects(params IObject3D[] objects3D)
        {
            return ContainObjectsArray(objects3D);
        }

        //TODO: test to add hierarchy chain in every cases
        public virtual void AddObject(in IObject3D object3D)
        {
            if (object3D == null)
            {
                throw new ArgumentException("You can not add null object");
            }

            if (!ContainObject(object3D))
            {
                AttachedObjects.Add(object3D);

                ITransform transform = object3D.ThisTransform;
                IHierarchyManager<ITransform> hierarchy = transform.Hierarchy;

                if (hierarchy.Parent == null || !ContainObject(hierarchy.Parent.ParentObject))
                {
                    SceneTransform.Hierarchy.AddChild(transform);
                }

                foreach (ITransform child in hierarchy.Children)
                {
                    AddObject(child.ParentObject);
                }

                OnChanged();
            }
        }

        private void AddObjectsArray(in IObject3D[] objects3D)
        {
            foreach (IObject3D object3D in objects3D)
            {
                AddObject(object3D);
            }
        }

        public void AddObjects(in IObject3D[] objects3D)
        {
            AddObjectsArray(objects3D);
        }

        public void AddObjects(params IObject3D[] objects3D)
        {
            AddObjectsArray(objects3D);
        }

        //TODO: test to remove hierarchu chain in every case
        public virtual void RemoveObject(in IObject3D object3D)
        {
            if (object3D == null)
            {
                throw new ArgumentException("You can not remove null object");
            }

            if (ContainObject(object3D))
            {
                AttachedObjects.Remove(object3D);

                ITransform transform = object3D.ThisTransform;
                IHierarchyManager<ITransform> hierarchy = transform.Hierarchy;

                if (hierarchy.IsChildOf(SceneTransform))
                {
                    SceneTransform.Hierarchy.RemoveChild(transform);
                }

                foreach (ITransform child in hierarchy.Children)
                {
                    RemoveObject(child.ParentObject);
                }

                OnChanged();
            }
        }

        private void RemoveObjectsArray(in IObject3D[] objects3D)
        {
            foreach (IObject3D object3D in objects3D)
            {
                RemoveObject(object3D);
            }
        }

        public void RemoveObjects(in IObject3D[] objects3D)
        {
            RemoveObjectsArray(objects3D);
        }

        public void RemoveObjects(params IObject3D[] objects3D)
        {
            RemoveObjectsArray(objects3D);
        }

        public IReadOnlyList<IObject3D> FindObjectsByName(string name)
        {
            Predicate<IObject3D> predicate = (object3D) => object3D.Name == name;

            return AttachedObjects.FindAll(predicate);
        }

        public IReadOnlyList<IObject3D> FindObjectsByComponent(IComponent component)
        {
            Predicate<IObject3D> predicate = (object3D) => object3D.ContainComponent(component);

            return AttachedObjects.FindAll(predicate);
        }
    }
}
