using ConsoleGraphicEngine.Engine;
using ConsoleGraphicEngine.Engine.ConsoleSetter;
using ConsoleGraphicEngine.Engine.Objects;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Scenes;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleGraphicEngine
{
    class Program
    {
        static void Main()
        {
            ConsoleManager.MaximizeConsole();

            GraphicEngine engine = new GraphicEngine(InitializeScene());
            engine.StartRendering();

            //VisibleObject sphere = new VisibleObject(new SphereObjectRenderer(1), "Sphere");
            //sphere.transform.position = new Vector3(4, 0, 5);

            //Ray ray = new Ray(new Vector3(1, 0, 4), Vector3.Normalize(new Vector3(1, 0, 0)));
            //IReadOnlyList<Vector3> intersections = sphere.renderer.Intersect(ray);

            //if (intersections != null)
            //{
            //    Console.WriteLine($"Intersecions {intersections.Count}: ");

            //    foreach (Vector3 intersection in intersections)
            //    {
            //        Console.WriteLine(intersection);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No intersecions");
            //}
        }

        private static IScene InitializeScene()
        {
            Vector2Int consoleSize = new Vector2Int(Console.WindowWidth, Console.WindowHeight);

            IObject3D camera = new Object3D();
            camera.AddComponent(
                new Camera(consoleSize, 10,
                    new char[] { ' ', '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' }
                )
            );
            camera.transform.position = new Vector3(0, 0, 10);
            camera.transform.Rotate(new Vector3(0, 1, 0), (float)Math.PI / 2);

            IObject3D sphere = new VisibleObject(new SphereObjectRenderer(1), "Sphere");
            sphere.transform.position = new Vector3();

            Scene scene = new Scene();
            scene.AddObjects(
                camera,
                sphere
            );

            return scene;
        }
    }
}
