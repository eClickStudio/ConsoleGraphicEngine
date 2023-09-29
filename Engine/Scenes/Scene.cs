using Engine3D.ChangeTriggers;
using Engine3D.Components.Abstract;
using Engine3D.Components.Transform;
using Engine3D.Objects;
using System;
using System.Collections.Generic;

namespace Engine3D.Scenes
{
    public class Scene : AbstractChangebleUpdateble, IScene
    {
        protected List<IObject3D> AttachedObjects { get; }

        public IReadOnlyList<IObject3D> Objects => AttachedObjects;

        public Scene()
        {
            AttachedObjects = new List<IObject3D>();

            ChangableUpdatebleChildren = AttachedObjects;
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


        public virtual void RemoveObject(in IObject3D object3D)
        {
            if (object3D == null)
            {
                throw new ArgumentException("You can not remove null object");
            }

            if (ContainObject(object3D))
            {
                AttachedObjects.Remove(object3D);

                foreach (ITransform child in object3D.ThisTransform.Hierarchy.Children)
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
