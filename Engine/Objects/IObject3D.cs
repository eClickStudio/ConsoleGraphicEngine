using Engine3D.ChangeTriggers;
using Engine3D.Components.Abstract;
using Engine3D.Components.Transform;
using System.Collections.Generic;

namespace Engine3D.Objects
{
    public interface IObject3D : IChangebleUpdateble
    {
        /// <summary>
        /// Transform component of this object
        /// </summary>
        ITransform Transform { get; }

        /// <summary>
        /// Name of this object
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Is component attached to this object?
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        bool ContainComponent(in IComponent component);

        /// <summary>
        /// Is components attached to this object?
        /// </summary>
        /// <param name="component">Components array</param>
        /// <returns></returns>
        bool ContainComponents(in IComponent[] components);

        /// <summary>
        /// Is components attached to this object?
        /// </summary>
        /// <param name="component">Components</param>
        /// <returns></returns>
        bool ContainComponents(params IComponent[] components);

        /// <summary>
        /// Is componentType attached to this object?
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns></returns>
        bool ContainComponent<T>() where T : class, IComponent;


        /// <summary>
        /// Get component by its type
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns></returns>
        T GetComponent<T>() where T : class, IComponent;

        /// <summary>
        /// Get component by its type
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">Attached component</param>
        /// <returns>Is component attached to this object?</returns>
        bool TryGetComponent<T>(out T component) where T : class, IComponent;

        /// <summary>
        /// Get all components attached to this object
        /// </summary>
        /// <returns>All components</returns>
        IReadOnlyList<IComponent> GetComponents();


        /// <summary>
        /// Adds component to this object
        /// </summary>
        /// <param name="component">Component you want to add</param>
        void AddComponent(in IComponent component);

        /// <summary>
        /// Adds components to this object
        /// </summary>
        /// <param name="component">Components array you want to add</param>
        void AddComponents(in IComponent[] components);

        /// <summary>
        /// Adds components to this object
        /// </summary>
        /// <param name="component">Components you want to add</param>
        void AddComponents(params IComponent[] components);


        /// <summary>
        /// Removes component from this object
        /// </summary>
        /// <param name="component">Component you want to remove</param>
        void RemoveComponent(in IComponent component);

        /// <summary>
        /// Removes components from this object
        /// </summary>
        /// <param name="component">Components array you want to remove</param>
        void RemoveComponents(in IComponent[] components);

        /// <summary>
        /// Removes components from this object
        /// </summary>
        /// <param name="component">Components you want to remove</param>
        void RemoveComponents(params IComponent[] components);
    }
}
