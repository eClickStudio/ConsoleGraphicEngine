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
            commandManager.HandleCommand("start");

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

            IObject3D globalLight = SampleObjectsFactory.GetGlobalLight("GlobalLight", 0.1f);
            scene.AddObject(globalLight);
            scene.GlobalLight = globalLight.GetComponent<Light>();

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

            //Screen-DEBUG--------------------------------------------------------------------------
            Console.WriteLine("Screen");
            Console.WriteLine($"Resolution = {scene.MainCamera.Resolution}");

            //light---------------------------------------------------------------------------------
            IObject3D directionLight = SampleObjectsFactory.GetDirectionLight("DirectionLight", new Vector3(1, 1, 1), 1f);
            directionLight.AddComponent(new TransformRotator(new Vector3(-1, -1, 1), -30));

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
            //camera.AddComponent(new TransformRotator(true, box.Transform, new Vector3(1, 1, 1), -30));

            IObject3D sphere = SampleObjectsFactory.GetSphere("Sphere", Material.Solid, 1);
            sphere.Transform.Position = new Vector3(3, 0, 0);

            IObject3D sphereLight = SampleObjectsFactory.GetSphereLight("SphereLight", 0.7f, 1);
            sphereLight.Transform.Position = new Vector3(3, 0, 0);

            IObject3D plane = SampleObjectsFactory.GetPlane("Plane", Material.PerfectMirror, new Vector3(0, 0, 1), 0);

            //scene---------------------------------------------------------------------------------
            scene.AddObjects(
                directionLight,
                box
                );

            return scene;
        }
    }
}