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
        static async Task Main()
        {
            bool isWorking = true;

            ConsoleManager.MaximizeConsole();

            RayTracingGraphicEngine engine = new RayTracingGraphicEngine(5, 3);
            engine.RenderingScene = InitializeScene();

            CommandHandler commandManager = new CommandHandler(ConsoleColorSet.BlackGreen);
            commandManager.AddCommand("start", new Command("Strats engine rendering real time", engine.StartRenderingRealTime));
            commandManager.AddCommand("stop", new Command("Stops engine rendering real time", async () => await engine.StopRenderingRealTime()));
            commandManager.AddCommand("frame", new Command("Stops engine rendering real time and renders only one frame", async () => { await engine.StopRenderingRealTime(); engine.RenderFrame(); }));
            commandManager.AddCommand("hierarchy", new Command("Prints local scene hierarchy to the console", engine.RenderingScene.SceneTransform.Hierarchy.PrintHierarchy));
            commandManager.AddCommand("exit", new Command("Exits from this program", () => { isWorking = false; }));

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Its raytracingEngine command manager;\n" + 
                              "Type [command] to handle it;");
            commandManager.HandleCommand("help");

            string command;
            while (isWorking)
            {
                if (engine.IsRenderingRealTime)
                {
                    Console.ReadKey(true);
                    await engine.StopRenderingRealTime();
                }

                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.Green;
                command = Console.ReadLine().ToLower();
                Console.ForegroundColor = ConsoleColor.White;

                if (commandManager.ContainCommand(command))
                {
                    commandManager.HandleCommand(command);
                }
                else
                {
                    Console.WriteLine("Command you want to handle does not exist;\n" +
                                     $"Command you typed {command};\n" +
                                      "Type 'help' to get all addmissible commands;");
                }
            }
        }

        private static IRenderableScene<RayTracingCamera, ObjectRenderer> GetScene()
        {
            Vector2Int resolution = new Vector2Int(Console.WindowWidth, Console.WindowHeight);
            Vector2Int charSize = new Vector2Int(8, 16);
            CameraCharSet charSet = new CameraCharSet(' ',
                    new char[] { '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' });
            float resolutionAspect = resolution.X / resolution.Y;

            //Vector2 cameraAngle = new Vector2(
            //        (float)Math.PI * 2,
            //        (float)Math.PI * 2
            //    );

            Vector2 cameraAngle = new Vector2(
                    (float)Math.PI / 180 * 40 * resolutionAspect,
                    (float)Math.PI / 180 * 40
                );

            IRenderableScene<RayTracingCamera, ObjectRenderer> scene = new RenderableScene<RayTracingCamera, ObjectRenderer>();

            RayTracingCamera cameraComponent = new RayTracingCamera(resolution, charSize, cameraAngle, charSet);
            IObject3D camera = new Object3D("Camera");
            camera.AddComponent(cameraComponent);

            IDirectionLight globalLightComponent = new DirectionLight(new Vector3(1, -0.2f, 1), 1);
            IObject3D globalLight = new Object3D("GlobalLight");
            globalLight.AddComponent(globalLightComponent);

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

            IObject3D cube = new Object3D("Cube");
            cube.AddComponent(new CubeRenderer(Material.Solid, 1));

            float distanceFromCamera = 10;
            Vector3 cameraForwardPosition =
                camera.ThisTransform.Position + camera.ThisTransform.AxisZ * distanceFromCamera;

            cube.ThisTransform.Position = cameraForwardPosition;

            IObject3D sphere1 = new Object3D("Sphere_1");
            sphere1.AddComponent(new SphereRenderer(Material.Solid, 0.5f));
            sphere1.ThisTransform.Position = cameraForwardPosition + new Vector3(1, 0, -1);

            IObject3D sphere2 = new Object3D("Sphere_2");
            sphere2.AddComponent(new SphereRenderer(Material.Solid, 0.3f));
            sphere2.ThisTransform.Position = cameraForwardPosition + new Vector3(-1, 0, 1);

            //IObject3D someObject = new Object3D("SomeObject");
            //sphere2.ThisTransform.Hierarchy.AddChild(someObject.ThisTransform);

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