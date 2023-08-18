using ConsoleGraphicEngine.Engine;
using ConsoleGraphicEngine.Engine.ConsoleSetter;
using ConsoleGraphicEngine.Engine.Objects;
using ConsoleGraphicEngine.Engine.Objects.Components;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Scenes;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine
{
    class Program
    {
        static void Main()
        {
            ConsoleManager.MaximizeConsole();
            Vector2Int consoleSize = new Vector2Int(Console.WindowWidth, Console.WindowHeight);

            IObject3D camera = new Object3D();
            camera.AddComponent(
                new Camera(consoleSize,
                    new char[] { ' ', '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' }
                )
            );
            camera.transform.position = new Vector3(0, 0, 10);

            IObject3D sphere = new VisibleObject("Sphere");
            sphere.transform.position = new Vector3();

            Scene scene = new Scene();
            scene.AddObjects(
                camera,
                sphere
            );

            GraphicEngine engine = new GraphicEngine(scene);
            engine.StartRendering();
        }
    }
}
