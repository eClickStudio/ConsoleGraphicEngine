using ConsoleGraphicEngine.Engine.Basic.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Components.Transform;
using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Basic.Objects
{
    internal class Object3D : AbstractChangebleUpdateble, IObject3D
    {
        public ITransform transform { get; }
        public string name { get; set; }

        protected List<IComponent> components;

        public Object3D(in ITransform parent, string name = "noname_object")
        {
            this.name = name;
            components = new List<IComponent>();

            changableUpdatebleChildren = components;

            transform = new Transform(parent);
            AddComponent(transform);
        }


        public bool ContainComponent(in IComponent component)
        {
            return components.Contains(component);
        }

        public bool ContainComponents(in IComponent[] components)
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

        public bool ContainComponents(params IComponent[] components)
        {
            //TODO: test it may be stack owerflow

            return ContainComponents(components);
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

            return components.Find(predicate) as T;
        }

        public IReadOnlyList<T> GetComponents<T>() where T : class, IComponent
        {
            //TODO: test it

            Predicate<IComponent> predicate = (element) => element is T;

            return components.FindAll(predicate) as List<T>;
        }

        public IReadOnlyList<IComponent> GetComponents()
        {
            return components;
        }


        public void AddComponent(in IComponent component)
        {
            if (!components.Contains(component))
            {
                components.Add(component);
                component.parentObject = this;

                OnChanged();
            }
        }

        public void AddComponents(in IComponent[] components)
        {
            foreach (IComponent component in components)
            {
                AddComponent(component);
            }
        }

        public void AddComponents(params IComponent[] components)
        {
            AddComponents(components);
        }


        public void RemoveComponent(in IComponent component)
        {
            if (ContainComponent(component))
            {
                components.Remove(component);
                component.parentObject = null;

                OnChanged();
            }
        }

        public void RemoveComponents(in IComponent[] components)
        {
            foreach (IComponent component in components)
            {
                RemoveComponent(component);
            }
        }

        public void RemoveComponents(params IComponent[] components)
        {
            RemoveComponents(components);
        }
    }
}
