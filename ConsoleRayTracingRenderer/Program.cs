using Commands;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light;
using Engine3D.Objects;
using System.Numerics;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering;
using ConsoleSetter;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using RayTracingGraphicEngine3D;
using RayTracingGraphicEngine3D.RayTracingEngine.Scenes;
using RayTracingGraphicEngine3D.Samples;

//Scripts
using ConsoleRayTracingRenderer.Scripts;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;

namespace ConsoleRayTracingRenderer
{
    internal class Program
    {
        static async Task Main()
        {
            bool isWorking = true;

            ConsoleManager.MaximizeConsole();

            RayTracingGraphicEngine engine = new RayTracingGraphicEngine(0.1f);
            engine.LocalScene = GetLightTestScene();

            CommandHandler commandManager = new CommandHandler(ConsoleColorSet.BlackGreen);
            commandManager.AddCommand("start", new Command("Strats engine updating", engine.StartUpdating));
            commandManager.AddCommand("stop", new Command("Stops engine updating", async () => await engine.StopUpdating()));
            commandManager.AddCommand("update", new Command("Stops engine updating and updates only one frame", async () => { await engine.StopUpdating(); engine.UpdateFrame(); }));
            commandManager.AddCommand("frame", new Command("Stops engine updating and renders only one frame", async () => { await engine.StopUpdating(); engine.RenderFrame(); }));
            commandManager.AddCommand("hierarchy", new Command("Prints local scene hierarchy to the console", engine.LocalScene.SceneTransform.Hierarchy.PrintHierarchy));
            commandManager.AddCommand("maximize", new Command("Maximazes console", () => { ConsoleManager.MaximizeConsole(); commandManager.HandleCommand("frame"); }));
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
            RayTracingScene scene = new RayTracingScene(Material.Vacuum);

            IObject3D globalLight = SampleObjectsFactory.GetDirectionLight("DirectionLight", new Vector3(0, 0, 1), 1);

            scene.AddObject(globalLight);

            scene.GlobalLight = globalLight.GetComponent<DirectionLight>();

            //camera--------------------------------------------------------------------------------
            Vector2Int resolution = new Vector2Int(Console.WindowWidth, Console.WindowHeight);
            Vector2Int charSize = new Vector2Int(8, 16);
            CameraCharSet charSet = new CameraCharSet(' ',
                    new char[] { ' ', '.', ':', '!', '/', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' });
            float resolutionAspect = resolution.X / resolution.Y;

            //fishEyeCamera------------------------------------------------------------------------ -
            float angle = 40;

            Vector2 cameraAngle = new Vector2(
                    (float)Math.PI / 180 * angle * resolutionAspect,
                    (float)Math.PI / 180 * angle
                );
            IObject3D fishEyeCamera = SampleObjectsFactory.GetFishEyeCamera("FishEyeCamera", resolution, charSize, cameraAngle, charSet);

            scene.AddObject(fishEyeCamera);
            scene.MainCamera = fishEyeCamera.GetComponent<FishEyeCamera>();

            ////orthogonalCamera----------------------------------------------------------------------
            //float size = 10;

            //Vector2 cameraSize = new Vector2(
            //        size * resolutionAspect,
            //        size
            //    );
            //IObject3D orthogonalCamera = SampleObjectsFactory.GetOrthogonalCamera("OrthogonalCamera", resolution, charSize, cameraSize, charSet);

            //scene.AddObject(orthogonalCamera);
            //scene.MainCamera = orthogonalCamera.GetComponent<OrthogonalCamera>();

            //scene---------------------------------------------------------------------------------
            return scene;
        }

        private static RayTracingScene GetSolarSystemScene()
        {
            RayTracingScene scene = GetScene();

            //camera--------------------------------------------------------------------------------
            IObject3D camera = scene.MainCamera.ParentObject;
            camera.Transform.Position = new Vector3(0, 0, 50);
            camera.Transform.DirectAxisByPosition(Vector3.UnitZ, new Vector3(0, 0, 0));

            //objects-------------------------------------------------------------------------------
            IObject3D sun = SampleObjectsFactory.GetSphereLight("Solar", 0.95f, 1.5f);

            //IObject3D planet = SampleObjectsFactory.GetSphere("Planet", Material.Solid, 1);
            IObject3D planet = SampleObjectsFactory.GetSphereLight("Planet", 0.7f, 1);
            planet.Transform.Position = new Vector3(5, 0, 0);
            planet.AddComponent(new TransformRotator(true, sun.Transform, Vector3.UnitZ, 30));

            //IObject3D satellite = SampleObjectsFactory.GetSphere("Satellite", Material.Solid, 0.5f);
            IObject3D satellite = SampleObjectsFactory.GetSphereLight("Satellite", 0.5f, 0.5f);
            satellite.Transform.Position = new Vector3(3, 0, 0);
            satellite.AddComponent(new TransformRotator(true, planet.Transform, Vector3.UnitZ, 30));

            //scene---------------------------------------------------------------------------------
            scene.AddObjects(
                sun,
                planet,
                satellite
                );

            return scene;
        }

        private static RayTracingScene GetLightTestScene()
        {
            RayTracingScene scene = GetScene();

            //light---------------------------------------------------------------------------------
            IDirectionLight globalLight = scene.GlobalLight;
            globalLight.LocalDirection = new Vector3(0, 1, -1);
            globalLight.Intensity = 1;
            globalLight.ParentObject.AddComponent(new TransformRotator(new Vector3(1, 1, 1), 30));

            Console.WriteLine($"GlobalLightWorldDirection = {globalLight.WorldDirection}");

            //camera--------------------------------------------------------------------------------
            IObject3D camera = scene.MainCamera.ParentObject;
            camera.Transform.Position = new Vector3(5, 5, 5);
            camera.Transform.DirectAxisByPosition(Vector3.UnitZ, new Vector3(0, 0, 0));

            Console.WriteLine($"CameraAxisX = {camera.Transform.AxisX}");
            Console.WriteLine($"CameraAxisY = {camera.Transform.AxisY}");
            Console.WriteLine($"CameraAxisZ = {camera.Transform.AxisZ}");

            //objects-------------------------------------------------------------------------------
            IObject3D box = SampleObjectsFactory.GetBox("Box", Material.Solid, new Vector3(2, 2, 2));
            box.Transform.Position = new Vector3(0, 0, 0);
            camera.AddComponent(new TransformRotator(true, box.Transform, new Vector3(0, 1, 1), -30));

            IObject3D sphere = SampleObjectsFactory.GetSphere("Sphere", Material.Solid, 1);
            sphere.Transform.Position = new Vector3(3, 0, 0);

            //scene---------------------------------------------------------------------------------
            scene.AddObjects(
                sphere,
                box
                );

            return scene;
        }

        //private static RayTracingScene InitializeScene()
        //{
        //    //TODO: set charSize automaticly

        //    //RayTracingScene scene = GetScene();

        //    //IObject3D camera = scene.MainCamera.ParentObject;
        //    //camera.Transform.Position = new Vector3(0, 10, 0);
        //    //camera.Transform.RotateAroundAxis(new Vector3(0, 1, 0), (float)Math.PI / 4);
        //    //camera.Transform.RotateAroundAxis(new Vector3(-1, 0, 1), -(float)Math.PI / 4);

        //    //IObject3D cube = SampleObjectsFactory.GetCube("Cube", Material.Solid, 1);

        //    //float distanceFromCamera = 10;
        //    //Vector3 cameraForwardPosition =
        //    //    camera.Transform.Position + camera.Transform.AxisZ * distanceFromCamera;

        //    //cube.Transform.Position = cameraForwardPosition;

        //    //IObject3D sphere1 = SampleObjectsFactory.GetSphere("Sphere1", Material.Diamond, 0.5f);
        //    //sphere1.Transform.Position = cameraForwardPosition + new Vector3(1, 0, -1);

        //    //IObject3D sphereLight = SampleObjectsFactory.GetSphereLight("SphereLight", 1, 0.7f);
        //    //sphereLight.Transform.Position = cameraForwardPosition + new Vector3(-1, 0, 1);

        //    ////IObject3D someObject = new Object3D("SomeObject");
        //    ////sphere2.ThisTransform.Hierarchy.AddChild(someObject.ThisTransform);

        //    //camera.AddComponent(new TransformRotator(true, cube.Transform, new Vector3(0, 1, 0), 10));

        //    //scene.AddObjects(
        //    //        cube,
        //    //        sphere1,
        //    //        sphereLight
        //    //    );

        //    RayTracingScene scene = GetScene();

        //    IObject3D camera = scene.MainCamera.ParentObject;
        //    camera.Transform.Position = new Vector3(0, 0, 0);

        //    //camera.Transform.RotateAroundAxis(new Vector3(0, 1, 0), (float)Math.PI / 4);
        //    //camera.Transform.RotateAroundAxis(new Vector3(-1, 0, 1), -(float)Math.PI / 4);

        //    //float shphereDistanceFromCamera = 10;
        //    IObject3D sphere = SampleObjectsFactory.GetSphere("Sphere", Material.Solid, 1f);
        //    //sphere.Transform.Position = camera.Transform.Position + camera.Transform.AxisZ * shphereDistanceFromCamera;
        //    sphere.Transform.Position = new Vector3(0, 0, 5);

        //    IObject3D cube = SampleObjectsFactory.GetCube("Cube", Material.Diamond, 1f);
        //    cube.Transform.Position = new Vector3(0, 0, 5);
        //    camera.AddComponent(new TransformRotator(true, sphere.Transform, new Vector3(1, 1, 1), 30));

        //    float shphereLightDistanceFromCamera = 15;
        //    IObject3D sphereLight = SampleObjectsFactory.GetSphereLight("SphereLight", 1, 2);
        //    //sphereLight.Transform.Position = camera.Transform.Position + camera.Transform.AxisZ * shphereLightDistanceFromCamera;
        //    //sphereLight.Transform.Position = new Vector3(sphereLight.Transform.Position.X - 1, sphereLight.Transform.Position.Y, sphereLight.Transform.Position.Z);
        //    sphereLight.Transform.Position = new Vector3(0, 0, 9);
        //    //sphereLight.AddComponent(new TransformMover(new Vector3(0, 2, 5), new Vector3(0, -2, 5), 0.5f));

        //    //IObject3D plane = SampleObjectsFactory.GetPlaneLight("Plane", 1, new Vector3(0, 1, 0), 0);

        //    //IObject3D someObject = new Object3D("SomeObject");
        //    //sphere2.ThisTransform.Hierarchy.AddChild(someObject.ThisTransform);

        //    //camera.AddComponent(new TransformRotator(true, sphere.Transform, new Vector3(0, 1, 0), 10));
        //    //plane.AddComponent(new TransformRotator(false, plane.Transform, new Vector3(0.5f, 0.5f, 0.5f), 10));

        //    scene.AddObjects(
        //            sphere
        //            //cube,
        //            //plane,
        //            //sphereLight
        //        );

        //    return scene;
        //}
    }
}