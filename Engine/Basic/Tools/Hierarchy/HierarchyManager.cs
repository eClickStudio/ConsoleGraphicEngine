using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Basic.Tools.Hierarchy
{
    internal class HierarchyManager<T> : IHierarchyManager<T>
        where T : class, IHierarchyMember<T>
    {
        private T _hierarchyParent { get; }
        public T parent { get; private set; }

        private List<T> _children;
        public IReadOnlyList<T> children => _children;

        public int childrenCount => _children.Count;

        public int hierarchyIndex => parent.hierarchy.IndexOfChild(_hierarchyParent).Value;

        public event Action onChanged;

        public HierarchyManager(in T hierarchyParent, in T parent)
        {
            _hierarchyParent = hierarchyParent;
            SetParent(parent);

            _children = new List<T>();
        }

        public void AddChild(in T child)
        {
            if (!HasChild(child))
            {
                _children.Add(child);
                child.hierarchy.SetParent(_hierarchyParent);

                onChanged?.Invoke();
            }
        }

        public void AddChildren(in T[] children)
        {
            bool didChange = false;

            foreach (T child in children)
            {
                if (!HasChild(child))
                {
                    didChange = true;

                    _children.Add(child);
                    child.hierarchy.SetParent(_hierarchyParent);
                }
            }

            if (didChange)
            {
                onChanged?.Invoke();
            }
        }

        public bool HasChild(in T child)
        {
            return _children.Contains(child);
        }

        public int? IndexOfChild(T child)
        {
            if (!HasChild(child))
            {
                return null;
            }

            return _children.IndexOf(child);
        }

        public bool IsChildOf(in T parent)
        {
            return this.parent == parent;
        }

        public void RemoveChild(in T child)
        {
            if (HasChild(child))
            {
                _children.Remove(child);

                onChanged?.Invoke();
            }
        }

        public void RemoveChild(int index)
        {
            if (childrenCount > index)
            {
                _children.RemoveAt(index);
                    
                onChanged?.Invoke();
            }
        }

        public void SetParent(in T parent)
        {
            if (this.parent != parent)
            {
                this.parent = parent;

                onChanged?.Invoke();
            }
        }
    }
}
