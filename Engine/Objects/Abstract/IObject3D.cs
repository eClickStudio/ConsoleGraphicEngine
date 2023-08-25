using ConsoleGraphicEngine.Engine.Objects.Components;
using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Objects.Abstract
{
    interface IObject3D
    {
        public Transform transform { get; }
        string name { get; set; }

        bool ContainsComponent(in Component component);
        bool ContainsComponent<T>() where T : Component;

        T GetComponent<T>() where T : Component;
        bool TryGetComponent<T>(out T component) where T : Component;
        IReadOnlyList<Component> GetComponents();

        void AddComponent(in Component component);
        void AddComponents(in Component[] components);

        void RemoveComponent(in Component component);
        void RemoveComponents(in Component[] components);
    }
}
