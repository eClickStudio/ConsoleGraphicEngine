using ConsoleGraphicEngine.Engine.Objects.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.Light;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Objects.Scenes
{
    interface IScene
    {
        Camera mainCamera { get; }
        GlobalLight globalLight { get; }

        bool ContainsObject(in IObject3D object3D);

        void AddObject(in IObject3D object3D);
        void AddObjects(in IObject3D[] objects3D);
        void AddObjects(params IObject3D[] objects3D);

        void RemoveObject(in IObject3D object3D);

        IReadOnlyList<IObject3D> FindObjectsByName(string name);
        IReadOnlyList<IObject3D> FindObjectsByComponent(Component component);

        IReadOnlyList<IObject3D> GetObjects();
        IReadOnlyList<IVisibleObject> GetVisibleObjects();
    }
}
