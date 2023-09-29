using System;
using System.Collections.Generic;

namespace Hierarchy
{
    public class HierarchyManager<T> : IHierarchyManager<T>
        where T : class, IHierarchyMember<T>
    {
        protected T HierarchyParent { get; }
        public T Parent { get; private set; }

        private readonly List<T> _children;

        public IReadOnlyList<T> Children => _children;

        public int ChildrenCount => _children.Count;

        public int IndexInHierarchy => Parent.Hierarchy.IndexOfChild(HierarchyParent).Value;

        public event Action OnHierarchyChangedEvent;

        public HierarchyManager(in T hierarchyParent, in T parent)
        {
            HierarchyParent = hierarchyParent;
            SetParent(parent);

            _children = new List<T>();
        }

        public void AddChild(in T child)
        {
            if (!HasChild(child))
            {
                _children.Add(child);
                child.Hierarchy.SetParent(HierarchyParent);

                OnHierarchyChangedEvent?.Invoke();
            }
        }

        public void AddChildren(in T[] children)
        {
            foreach (T child in children)
            {
                if (!HasChild(child))
                {
                    _children.Add(child);
                    child.Hierarchy.SetParent(HierarchyParent);
                }
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
            return Parent == parent;
        }

        public void RemoveChild(in T child)
        {
            if (HasChild(child))
            {
                _children.Remove(child);

                OnHierarchyChangedEvent?.Invoke();
            }
        }

        public void RemoveChild(int index)
        {
            if (ChildrenCount > index)
            {
                _children.RemoveAt(index);

                OnHierarchyChangedEvent?.Invoke();
            }
        }

        public void SetParent(in T parent)
        {
            if (Parent != parent)
            {
                Parent = parent;

                OnHierarchyChangedEvent?.Invoke();
            }
        }
    }
}
