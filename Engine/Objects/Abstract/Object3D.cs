using ConsoleGraphicEngine.Engine.Objects.Components;
using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Objects.Abstract
{
    internal class Object3D : IObject3D
    {
        public Transform transform { get; }
        public string name { get; set; }

        protected List<Component> components;

        public Object3D(string name = "noname_object")
        {
            this.name = name;
            components = new List<Component>();

            transform = new Transform();

            AddComponent(transform);
        }

        public void AddComponent(in Component component)
        {
            if (!components.Contains(component))
            {
                components.Add(component);
                component.parentObject = this;
            }
        }

        public void AddComponents(in Component[] components)
        {
            foreach (Component component in components)
            {
                AddComponent(component);
            }
        }

        public T GetComponent<T>() where T : Component
        {
            Predicate<IComponent> predicate = (element) => element is T;

            return components.Find(predicate) as T;
        }

        public IReadOnlyList<Component> GetComponents()
        {
            return components;
        }

        public bool ContainsComponent(in Component component)
        {
            return components.Contains(component);
        }

        public bool ContainsComponent<T>() where T : Component
        {
            return GetComponent<T>() != null;
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();

            return component != null;
        }

        public void RemoveComponent(in Component component)
        {
            if (ContainsComponent(component))
            {
                components.Remove(component);
            }
        }

        public void RemoveComponents(in Component[] components)
        {
            foreach (Component component in components)
            {
                RemoveComponent(component);
            }
        }
    }
}
