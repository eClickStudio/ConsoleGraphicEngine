using ConsoleGraphicEngine.Engine;
using ConsoleGraphicEngine.Engine.ConsoleSetter;
using ConsoleGraphicEngine.Engine.Objects;
using ConsoleGraphicEngine.Engine.Objects.Abstract;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.Light;
using ConsoleGraphicEngine.Engine.Objects.Components.Rendering.ObjectRenderers;
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
            //TODO: fix Oz bug its inversive

            ConsoleManager.MaximizeConsole();

            GraphicEngine engine = new GraphicEngine(InitializeScene(), 5);
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
            Vector2Int charSize = new Vector2Int(8, 16);

            IObject3D camera = new Object3D("Camera");
            camera.AddComponent(
                new Camera(consoleSize, charSize, 10, 1,
                    new char[] { ' ', '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' }
                )
            );
            camera.transform.position = new Vector3(10, 0, 0);
            camera.transform.Rotate(new Vector3(0, 1, 0), -(float)Math.PI / 2);

            IObject3D globalLight = new Object3D("GlobalLight");
            globalLight.AddComponent(new GlobalLight(new Vector3(1, -0.2f, 0), 1));

            IObject3D sphere1 = new VisibleObject(new SphereObjectRenderer(Material.standart, 1), "Sphere");
            sphere1.transform.position = new Vector3(0, 0, 0);

            IObject3D sphere2 = new VisibleObject(new SphereObjectRenderer(Material.standart, 0.5f), "Sphere");
            sphere2.transform.position = new Vector3(0.7f, 0.4f, 1);

            IObject3D sphere3 = new VisibleObject(new SphereObjectRenderer(Material.standart, 0.3f), "Sphere");
            sphere3.transform.position = new Vector3(0.7f, 0.4f, -1);

            Scene scene = new Scene();
            scene.AddObjects(
                camera,
                globalLight,
                sphere1,
                sphere2,
                sphere3
            );

            return scene;
        }
    }
}
