using ConsoleGraphicEngine.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine.Engine.Basic.ConsoleSetter;
using ConsoleGraphicEngine.Engine.Basic.Objects;
using ConsoleGraphicEngine.Engine.Basic.Scenes;
using ConsoleGraphicEngine.Engine.Basic.Tools;
using ConsoleGraphicEngine.Engine.RayTracingEngine;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Camera;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers;
using ConsoleGraphicEngine.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine
{
    class Program
    {
        static void Main()
        {
            ConsoleManager.MaximizeConsole();

            RayTracingGraphicEngine engine = new RayTracingGraphicEngine(5, 3);
            engine.scene = InitializeScene();
            engine.StartRenderingRealTime();
        }

        private static IScene<RayTracingCamera, ObjectRenderer> InitializeScene()
        {
            //TODO: set charSize automaticly

            Vector2Int consoleSize = new Vector2Int(Console.WindowWidth, Console.WindowHeight);
            Vector2Int charSize = new Vector2Int(8, 16);
            CameraCharSet charSet = new CameraCharSet(' ',
                    new char[] { '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' } );

            IObject3D camera = new Object3D(null, "Camera");
            camera.AddComponent(new RayTracingCamera(consoleSize, charSize, 10, 1, charSet));
            //camera.transform.position = new Vector3(10, 0, 0);
            //camera.transform.Rotate(new Vector3(0, 1, 0), -(float)Math.PI / 2);

            camera.transform.position = new Vector3(0, 10, 0);
            camera.transform.Rotate(new Vector3(0, 1, 0), (float)Math.PI / 4);
            camera.transform.Rotate(new Vector3(-1, 0, 1), -(float)Math.PI / 4);

            IObject3D globalLight = new Object3D("GlobalLight");
            globalLight.AddComponent(new GlobalLight(new Vector3(1, -0.2f, 1), 1));

            float objectSize = 1f;
            ObjectRenderer renderer = new BoxRenderer(Material.standart, Vector3.One * objectSize);
            //ObjectRenderer renderer = new SphereObjectRenderer(Material.standart, 1);
            IObject3D centralObject = new VisibleObject(renderer);

            float distanceFromCamera = 10;
            Vector3 cameraForwardPosition = 
                camera.transform.position + camera.transform.axisZ * distanceFromCamera;

            centralObject.transform.position = cameraForwardPosition + new Vector3(0, 0, 0);

            Console.WriteLine(camera.transform.axisZ * distanceFromCamera);

            //IObject3D sphere1 = new VisibleObject(new SphereObjectRenderer(Material.standart, 1), "Sphere");
            //sphere1.transform.position = new Vector3(0, 0, 0);

            IObject3D sphere2 = new VisibleObject(new SphereRenderer(Material.standart, 0.5f), "Sphere");
            sphere2.transform.position = cameraForwardPosition + new Vector3(1, 0, -1);

            IObject3D sphere3 = new VisibleObject(new SphereRenderer(Material.standart, 0.3f), "Sphere");
            sphere3.transform.position = cameraForwardPosition + new Vector3(-1, 0, 1);

            Scene scene = new Scene();
            scene.AddObjects(
                camera,
                globalLight,
                centralObject,
                sphere2,
                sphere3
            );

            return scene;
        }
    }
}
