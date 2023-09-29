using Commands;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Camera;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Light;
using ConsoleGraphicEngine3D.Engine.Basic.Components.Rendering;
using ConsoleGraphicEngine3D.Engine.Basic.ConsoleSetter;
using ConsoleGraphicEngine3D.Engine.Basic.Scenes;
using ConsoleGraphicEngine3D.Engine.Basic.Tools;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Camera;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Light;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers;
using ConsoleGraphicEngine3D.Engine.RayTracingEngine.Components.Rendering.ObjectRenderers.Abstract;
using Engine3D.Objects;
using System.Numerics;

//Scripts
using ConsoleRayTracingRenderer.Scripts;

namespace ConsoleRayTracingRenderer
{
    internal class Program
    {
        static void Main()
        {
            ConsoleManager.MaximizeConsole();

            RayTracingGraphicEngine engine = new RayTracingGraphicEngine(5, 3);
            engine.RenderingScene = InitializeScene();

            CommandManager commandManager = new CommandManager();
            commandManager.AddCommand("start", new Command("Strats engine rendering real time", engine.StartRenderingRealTime));
            commandManager.AddCommand("stop", new Command("Stops engine rendering real time", async () => await engine.StopRenderingRealTime()));
            commandManager.AddCommand("frame", new Command("Stops engine rendering real time and renders only one frame", async () => { await engine.StopRenderingRealTime(); engine.RenderFrame(); }));

            string command;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                engine.StopRenderingRealTime();

                Console.Write(key.ToString());
                command = key.ToString() + Console.ReadLine().ToLower();

                commandManager.HandleCommand(command);
            }
            while (command != "exit");
        }

        private static IRenderableScene<RayTracingCamera, ObjectRenderer> GetScene()
        {
            Vector2Int resolution = new Vector2Int(Console.WindowWidth, Console.WindowHeight);
            Vector2Int charSize = new Vector2Int(8, 16);
            CameraCharSet charSet = new CameraCharSet(' ',
                    new char[] { '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' });
            float resolutionAspect = resolution.X / resolution.Y;

            Vector2 cameraAngle = new Vector2(
                    (float)Math.PI * 2,
                    (float)Math.PI * 2
                );

            //Vector2 cameraAngle = new Vector2(
            //        (float)Math.PI / 180 * 40 * resolutionAspect,
            //        (float)Math.PI / 180 * 40
            //    );

            RayTracingCamera cameraComponent = new RayTracingCamera(resolution, charSize, cameraAngle, charSet);
            IObject3D camera = new Object3D(null, "Camera");
            camera.AddComponent(cameraComponent);

            IDirectionLight globalLightComponent = new DirectionLight(new Vector3(1, -0.2f, 1), 1);
            IObject3D globalLight = new Object3D(null, "GlobalLight");
            globalLight.AddComponent(globalLightComponent);

            IRenderableScene<RayTracingCamera, ObjectRenderer> scene =
                new RenderableScene<RayTracingCamera, ObjectRenderer>();
            scene.AddObjects(
                camera,
                globalLight
            );

            scene.MainCamera = cameraComponent;
            scene.GlobalLight = globalLightComponent;

            return scene;
        }

        private static IRenderableScene<RayTracingCamera, ObjectRenderer> InitializeScene()
        {
            //TODO: set charSize automaticly

            IRenderableScene<RayTracingCamera, ObjectRenderer> scene = GetScene();

            IObject3D camera = scene.MainCamera.ParentObject;
            camera.ThisTransform.Position = new Vector3(0, 10, 0);
            camera.ThisTransform.RotateAroundAxis(new Vector3(0, 1, 0), (float)Math.PI / 4);
            camera.ThisTransform.RotateAroundAxis(new Vector3(-1, 0, 1), -(float)Math.PI / 4);

            IObject3D cube = new Object3D(null, "Cube");
            cube.AddComponent(new CubeRenderer(Material.Standart, 1));

            float distanceFromCamera = 10;
            Vector3 cameraForwardPosition =
                camera.ThisTransform.Position + camera.ThisTransform.AxisZ * distanceFromCamera;

            cube.ThisTransform.Position = cameraForwardPosition;

            IObject3D sphere1 = new Object3D(null, "Sphere_1");
            sphere1.AddComponent(new SphereRenderer(Material.Standart, 0.5f));
            sphere1.ThisTransform.Position = cameraForwardPosition + new Vector3(1, 0, -1);

            IObject3D sphere2 = new Object3D(null, "Sphere_2");
            sphere2.AddComponent(new SphereRenderer(Material.Standart, 0.3f));
            sphere2.ThisTransform.Position = cameraForwardPosition + new Vector3(-1, 0, 1);

            camera.AddComponent(new TransformRotator(true, cube.ThisTransform, new Vector3(0, 1, 0), 10));

            scene.AddObjects(
                    cube,
                    sphere1,
                    sphere2
                );

            return scene;
        }
    }
}