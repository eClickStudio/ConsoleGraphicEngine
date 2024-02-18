using System;
using System.Collections.Generic;

namespace Hierarchy
{
    /// <summary>
    /// The hierarchy interface
    /// </summary>
    /// <typeparam name="T">Type of hierarchy member</typeparam>
    public interface IReadOnlyHierarchyManager<T>
    {
        /// <summary>
        /// The parent of this
        /// </summary>
        T Parent { get; }

        /// <summary>
        /// Childs of this
        /// </summary>
        IReadOnlyList<T> Children { get; }

        /// <summary>
        /// The count of this hierarhy's children
        /// </summary>
        int ChildrenCount { get; }

        /// <summary>
        /// The count of all children (even the children of children hierarchy)
        /// </summary>
        int AllChildrenCount { get; }

        /// <summary>
        /// The count of childless children
        /// </summary>
        int ChildlessChildrenCount { get; }

        /// <summary>
        /// Triggers on some changes in hierarchy
        /// </summary>
        event Action OnHierarchyChangedEvent;

        /// <summary>
        /// Gets the childIndex
        /// </summary>
        /// <param name="child">The child</param>
        /// <returns>Index of child. If returs null there is no child in hierarchy</returns>
        int? IndexOfChild(T child);

        /// <summary>
        /// Cheks if there is a child
        /// </summary>
        /// <param name="child">The child</param>
        /// <returns>If there is a child?</returns>
        bool HasChild(in T child);

        /// <summary>
        /// The index in parent hierarchy
        /// </summary>
        int IndexInHierarchy { get; }

        /// <summary>
        /// Cheks if its the child of some parent
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>If its the child of the parent?</returns>
        bool IsChildOf(in T parent);

        /// <summary>
        /// Prints hierarchy to the console. Cannot print parents (prints only children)
        /// </summary>
        void PrintHierarchy();
    }
}
