using Commands;
using RayTracingGraphicEngine3D.Components.Camera;
using RayTracingGraphicEngine3D.Components.Light;
using Engine3D.Objects;
using System.Numerics;
using RayTracingGraphicEngine3D.Components.Rendering;
using ConsoleSetter;
using RayTracingGraphicEngine3D.Tools;
using RayTracingGraphicEngine3D;
using RayTracingGraphicEngine3D.Scenes;
using RayTracingGraphicEngine3D.Samples;

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

            RayTracingGraphicEngine engine = new RayTracingGraphicEngine(Material.Vacuum, 0.001f);
            engine.LocalScene = InitializeScene();

            CommandHandler commandManager = new CommandHandler(ConsoleColorSet.BlackGreen);
            commandManager.AddCommand("start", new Command("Strats engine updating", engine.StartUpdating));
            commandManager.AddCommand("stop", new Command("Stops engine updating", async () => await engine.StopUpdating()));
            commandManager.AddCommand("update", new Command("Stops engine updating and updates only one frame", async () => { await engine.StopUpdating(); engine.UpdateFrame(); }));
            commandManager.AddCommand("frame", new Command("Stops engine updating and renders only one frame", async () => { await engine.StopUpdating(); engine.RenderFrame(); }));
            commandManager.AddCommand("hierarchy", new Command("Prints local scene hierarchy to the console", engine.LocalScene.SceneTransform.Hierarchy.PrintHierarchy));
            commandManager.AddCommand("clear", new Command("Clears the console", () => { Console.Clear(); }));
            commandManager.AddCommand("exit", new Command("Exits from this program", () => { isWorking = false; }));

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Its raytracingEngine command manager;\n" + 
                              "Type [command] to handle it;");
            commandManager.HandleCommand("help");

            commandManager.HandleCommand("frame");

            string command;
            while (isWorking)
            {
                if (engine.IsUpdating)
                {
                    Console.ReadKey(true);
                    await engine.StopUpdating();
                }

                Console.Write(">>> ");
                Console.ForegroundColor = ConsoleColor.Green;
                command = Console.ReadLine()
                                 .ToLower()
                                 .Replace(" ", "");

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

        private static RayTracingScene GetScene()
        {
            Vector2Int resolution = new Vector2Int(Console.WindowWidth, Console.WindowHeight);
            Vector2Int charSize = new Vector2Int(8, 16);
            CameraCharSet charSet = new CameraCharSet(' ',
                    new char[] { ' ', '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' });
            float resolutionAspect = resolution.X / resolution.Y;

            //Vector2 cameraAngle = new Vector2(
            //        (float)Math.PI * 2,
            //        (float)Math.PI * 2
            //    );

            Vector2 cameraAngle = new Vector2(
                    (float)Math.PI / 180 * 40 * resolutionAspect,
                    (float)Math.PI / 180 * 40
                );

            RayTracingScene scene = new RayTracingScene();

            IObject3D camera = SampleObjectsFactory.GetCamera("Camera", resolution, charSize, cameraAngle, charSet);

            IObject3D globalLight = SampleObjectsFactory.GetDirectionLight("DirectionLight", new Vector3(1, -0.2f, 1), 1);

            scene.AddObjects(
                camera,
                globalLight
            );

            scene.MainCamera = camera.GetComponent<RayTracingCamera>();
            scene.GlobalLight = globalLight.GetComponent<DirectionLight>();

            return scene;
        }

        private static RayTracingScene InitializeScene()
        {
            //TODO: set charSize automaticly

            //RayTracingScene scene = GetScene();

            //IObject3D camera = scene.MainCamera.ParentObject;
            //camera.Transform.Position = new Vector3(0, 10, 0);
            //camera.Transform.RotateAroundAxis(new Vector3(0, 1, 0), (float)Math.PI / 4);
            //camera.Transform.RotateAroundAxis(new Vector3(-1, 0, 1), -(float)Math.PI / 4);

            //IObject3D cube = SampleObjectsFactory.GetCube("Cube", Material.Solid, 1);

            //float distanceFromCamera = 10;
            //Vector3 cameraForwardPosition =
            //    camera.Transform.Position + camera.Transform.AxisZ * distanceFromCamera;

            //cube.Transform.Position = cameraForwardPosition;

            //IObject3D sphere1 = SampleObjectsFactory.GetSphere("Sphere1", Material.Diamond, 0.5f);
            //sphere1.Transform.Position = cameraForwardPosition + new Vector3(1, 0, -1);

            //IObject3D sphereLight = SampleObjectsFactory.GetSphereLight("SphereLight", 1, 0.7f);
            //sphereLight.Transform.Position = cameraForwardPosition + new Vector3(-1, 0, 1);

            ////IObject3D someObject = new Object3D("SomeObject");
            ////sphere2.ThisTransform.Hierarchy.AddChild(someObject.ThisTransform);

            //camera.AddComponent(new TransformRotator(true, cube.Transform, new Vector3(0, 1, 0), 10));

            //scene.AddObjects(
            //        cube,
            //        sphere1,
            //        sphereLight
            //    );

            RayTracingScene scene = GetScene();

            IObject3D camera = scene.MainCamera.ParentObject;
            camera.Transform.Position = new Vector3(0, 10, 0);
            camera.Transform.RotateAroundAxis(new Vector3(0, 1, 0), (float)Math.PI / 4);
            camera.Transform.RotateAroundAxis(new Vector3(-1, 0, 1), -(float)Math.PI / 4);

            float shphereDistanceFromCamera = 10;
            IObject3D sphere = SampleObjectsFactory.GetSphere("Sphere", Material.Diamond, 1f);
            sphere.Transform.Position = camera.Transform.Position + camera.Transform.AxisZ * shphereDistanceFromCamera;

            float shphereLightDistanceFromCamera = 15;
            IObject3D sphereLight = SampleObjectsFactory.GetSphereLight("SphereLight", 1, 1);
            sphereLight.Transform.Position = camera.Transform.Position + camera.Transform.AxisZ * shphereLightDistanceFromCamera;

            IObject3D plane = SampleObjectsFactory.GetPlaneLight("Plane", 1, new Vector3(0, 1, 0), 0);

            //IObject3D someObject = new Object3D("SomeObject");
            //sphere2.ThisTransform.Hierarchy.AddChild(someObject.ThisTransform);

            //camera.AddComponent(new TransformRotator(true, sphere.Transform, new Vector3(0, 1, 0), 10));
            plane.AddComponent(new TransformRotator(false, plane.Transform, new Vector3(0.5f, 0.5f, 0.5f), 10));

            scene.AddObjects(
                    //sphere,
                    plane
                    //sphereLight
                );

            return scene;
        }
    }
}