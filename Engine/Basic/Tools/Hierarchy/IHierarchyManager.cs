using System.Collections.Generic;
using System;
using ConsoleGraphicEngine.Engine.Basic.Abstract;

namespace ConsoleGraphicEngine.Engine.Basic.Tools.Hierarchy
{
    /// <summary>
    /// The hierarchy interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IHierarchyManager<T>
        where T : class, IHierarchyMember<T>
    {
        /// <summary>
        /// The parent of this
        /// </summary>
        T parent { get; }

        /// <summary>
        /// Childs of this
        /// </summary>
        IReadOnlyList<T> children { get; }

        /// <summary>
        /// The count of childs
        /// </summary>
        int childrenCount { get; }

        /// <summary>
        /// Triggers on some changes in hierarchy
        /// </summary>
        event Action onHierarchyChanged;

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
        /// Adds child
        /// </summary>
        /// <param name="child">The child</param>
        void AddChild(in T child);

        /// <summary>
        /// Adds children
        /// </summary>
        /// <param name="children">The children</param>
        void AddChildren(in T[] children);

        /// <summary>
        /// Removes child
        /// </summary>
        /// <param name="child">The child</param>
        void RemoveChild(in T child);

        /// <summary>
        /// Removes child using index
        /// </summary>
        /// <param name="index">The index of child</param>
        void RemoveChild(int index);

        /// <summary>
        /// The index in parent hierarchy
        /// </summary>
        int indexInHierarchy { get; }

        /// <summary>
        /// Cheks if its the child of some parent
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>If its the child of the parent?</returns>
        bool IsChildOf(in T parent);

        /// <summary>
        /// Set the parent. If parent argument is null this will not be the child
        /// </summary>
        void SetParent(in T parent);
    }
}
