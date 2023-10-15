using Engine3D.ChangeTriggers;
using Engine3D.Components.Abstract;
using Engine3D.Components.Transform;
using System;
using System.Collections.Generic;

namespace Engine3D.Objects
{
    public class Object3D : AbstractChangebleUpdateble, IObject3D
    {
        public ITransform Transform { get; }
        public string Name { get; set; }

        protected readonly List<IComponent> Components;

        public Object3D(string name = "noname_object")
        {
            this.Name = name;
            Components = new List<IComponent>();

            ChangableUpdatebleChildren = Components;

            Transform = new Transform();
            AddComponent(Transform);
        }

        public Object3D(in ITransform transform, string name = "noname_object")
        {
            this.Name = name;
            Components = new List<IComponent>();

            ChangableUpdatebleChildren = Components;

            Transform = transform;
            AddComponent(Transform);
        }

        public bool ContainComponent(in IComponent component)
        {
            return Components.Contains(component);
        }

        private bool ContainComponentsArray(in IComponent[] components)
        {
            foreach (IComponent component in components)
            {
                if (!ContainComponent(component))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainComponents(in IComponent[] components)
        {
            return ContainComponentsArray(components);
        }

        public bool ContainComponents(params IComponent[] components)
        {
            return ContainComponentsArray(components);
        }

        public bool ContainComponent<T>() where T : class, IComponent
        {
            return GetComponent<T>() != null;
        }

        public bool TryGetComponent<T>(out T component) where T : class, IComponent
        {
            component = GetComponent<T>();

            return component != null;
        }


        public T GetComponent<T>() where T : class, IComponent
        {
            Predicate<IComponent> predicate = (element) => element is T;

            return Components.Find(predicate) as T;
        }

        public IReadOnlyList<T> GetComponents<T>() where T : class, IComponent
        {
            //TODO: test it

            Predicate<IComponent> predicate = (element) => element is T;

            return Components.FindAll(predicate) as List<T>;
        }

        public IReadOnlyList<IComponent> GetComponents()
        {
            return Components;
        }


        public void AddComponent(in IComponent component)
        {
            if (!Components.Contains(component))
            {
                Components.Add(component);
                component.ParentObject = this;

                OnChanged();
            }
        }

        private void AddComponentsArray(in IComponent[] components)
        {
            foreach (IComponent component in components)
            {
                AddComponent(component);
            }
        }

        public void AddComponents(in IComponent[] components)
        {
            AddComponentsArray(components);
        }

        public void AddComponents(params IComponent[] components)
        {
            AddComponentsArray(components);
        }


        public void RemoveComponent(in IComponent component)
        {
            if (ContainComponent(component))
            {
                Components.Remove(component);
                component.ParentObject = null;

                OnChanged();
            }
        }

        private void RemoveComponentsArray(in IComponent[] components)
        {
            foreach (IComponent component in components)
            {
                RemoveComponent(component);
            }
        }

        public void RemoveComponents(in IComponent[] components)
        {
            RemoveComponentsArray(components);
        }

        public void RemoveComponents(params IComponent[] components)
        {
            RemoveComponentsArray(components);
        }
    }
}
