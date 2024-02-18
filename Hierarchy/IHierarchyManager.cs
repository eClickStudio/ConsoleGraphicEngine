using System.Collections.Generic;
using System;

namespace Hierarchy
{
    /// <summary>
    /// The hierarchy interface
    /// </summary>
    /// <typeparam name="T">Type of hierarchy member</typeparam>
    public interface IHierarchyManager<T> : IReadOnlyHierarchyManager<T>
        where T : class, IHierarchyMember<T>
    {
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
        /// Set the parent. If parent argument is null this will not be the child
        /// </summary>
        void SetParent(in T parent);
    }
}
