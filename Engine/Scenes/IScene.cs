using Engine3D.ChangeTriggers;
using Engine3D.Components.Abstract;
using Engine3D.Components.Transform;
using Engine3D.Objects;
using System.Collections.Generic;

namespace Engine3D.Scenes
{
    public interface IScene : IChangebleUpdateble
    {
        /// <summary>
        /// Transform of this scene
        /// </summary>
        ITransform SceneTransform { get; }

        /// <summary>
        /// All scene objects
        /// </summary>
        IReadOnlyList<IObject3D> Objects { get; }

        /// <summary>
        /// Is there object on the scene
        /// </summary>
        /// <param name="object3D">Object</param>
        /// <returns></returns>
        bool ContainObject(in IObject3D object3D);

        /// <summary>
        /// Is there objects on the scene
        /// </summary>
        /// <param name="objects3D">Objects array</param>
        /// <returns></returns>
        bool ContainObjects(in IObject3D[] objects3D);

        /// <summary>
        /// Is there objects on the scene
        /// </summary>
        /// <param name="objects3D">Objects</param>
        /// <returns></returns>
        bool ContainObjects(params IObject3D[] objects3D);


        /// <summary>
        /// Adds object to the scene
        /// </summary>
        /// <param name="object3D">Object you want to add</param>
        void AddObject(in IObject3D object3D);

        /// <summary>
        /// Adds objects to the scene
        /// </summary>
        /// <param name="objects3D">Objects array you want to add</param>
        void AddObjects(in IObject3D[] objects3D);

        /// <summary>
        /// Adds objects to the scene
        /// </summary>
        /// <param name="objects3D">Objects you want to add</param>
        void AddObjects(params IObject3D[] objects3D);


        /// <summary>
        /// Removes object from the scene
        /// </summary>
        /// <param name="object3D">Object you want to remove</param>
        void RemoveObject(in IObject3D object3D);

        /// <summary>
        /// Removes objects from the scene
        /// </summary>
        /// <param name="objects3D">Objects array you want to remove</param>
        void RemoveObjects(in IObject3D[] objects3D);

        /// <summary>
        /// Removes objects from the scene
        /// </summary>
        /// <param name="objects3D">Objects you want to remove</param>
        void RemoveObjects(params IObject3D[] objects3D);


        /// <summary>
        /// Find all objects with this name
        /// </summary>
        /// <param name="name">Object name</param>
        /// <returns></returns>
        IReadOnlyList<IObject3D> FindObjectsByName(string name);

        /// <summary>
        /// Find all objects with this component
        /// </summary>
        /// <param name="component">Object component</param>
        /// <returns></returns>
        IReadOnlyList<IObject3D> FindObjectsByComponent(IComponent component);
    }
}
