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

namespace ConsoleRayTracingRenderer
{
    internal class Program
    {
        private static RayTracingGraphicEngine _engine;
        private static CommandHandler _commandManager;
        private static Dictionary<string, RayTracingScene> _scenes;

        static async Task Main()
        {
            bool isWorking = true;

            ConsoleManager.MaximizeConsole();

            _scenes = new Dictionary<string, RayTracingScene>()
            {
                { "SphereScene", GetSphereScene() },
                { "CubeScene", GetCubeScene() },
                { "SatelliteScene", GetSatelliteScene()},
                { "TwoCubesScene", GetTwoCubesScene()}
            };

            _engine = new RayTracingGraphicEngine(3, 0.1f);
            _engine.LocalScene = GetCubeScene();

            _commandManager = new CommandHandler(ConsoleColorSet.BlackGreen);
            _commandManager.AddCommand("load", new Command("LoadingScene", LoadCommand));
            _commandManager.AddCommand("start", new Command("Strats engine updating", _engine.StartUpdating));
            _commandManager.AddCommand("stop", new Command("Stops engine updating", async () => await _engine.StopUpdating()));
            _commandManager.AddCommand("update", new Command("Stops engine updating and updates only one frame", async () => { await _engine.StopUpdating(); _engine.UpdateFrame(); }));
            _commandManager.AddCommand("frame", new Command("Stops engine updating and renders only one frame", async () => { await _engine.StopUpdating(); _engine.RenderFrame(); }));
            _commandManager.AddCommand("hierarchy", new Command("Prints local scene hierarchy to the console", _engine.LocalScene.SceneTransform.Hierarchy.PrintHierarchy));
            _commandManager.AddCommand("maximize", new Command("Maximazes console", () => { ConsoleManager.MaximizeConsole(); _commandManager.HandleCommand("frame"); }));
            _commandManager.AddCommand("clear", new Command("Clears the console", () => { Console.Clear(); }));
            _commandManager.AddCommand("exit", new Command("Exits from this program", () => { isWorking = false; }));

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Its raytracingEngine command manager;\n" + 
                              "Type [command] to handle it;");
            _commandManager.HandleCommand("help");

            _commandManager.HandleCommand("frame");

            string command;
            while (isWorking)
            {
                if (_engine.IsUpdating)
                {
                    Console.ReadKey(true);
                    await _engine.StopUpdating();
                }

                Console.Write(">>> ");
                Console.ForegroundColor = ConsoleColor.Green;
                command = Console.ReadLine()
                                 .ToLower()
                                 .Replace(" ", "");

                Console.ForegroundColor = ConsoleColor.White;
                if (_commandManager.ContainCommand(command))
                {
                    _commandManager.HandleCommand(command);
                }
                else
                {
                    Console.WriteLine("Command you want to handle does not exist;\n" +
                                     $"Command you typed {command};\n" +
                                      "Type 'help' to get all addmissible commands;");
                }
            }
        }

        private static void LoadCommand()
        {
            Console.WriteLine("Type 'exit' to exit");
            Console.WriteLine("Type scene you want to load:");
            foreach(string key in _scenes.Keys)
            {
                Console.WriteLine($" * {key}");
            }

            while (true)
            {
                Console.Write(">>> ");
                string name = Console.ReadLine();

                if (name.ToLower() == "exit")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                if (name != null && _scenes.ContainsKey(name))
                {
                    Console.WriteLine("LoadingScene...");
                    _engine.LocalScene = _scenes[name];
                    Console.WriteLine();
                    _commandManager.HandleCommand("frame");
                    break;
                }
                else
                {
                    Console.WriteLine("Scene name is invalid. Please type name from list above");
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

        private static RayTracingScene GetSphereScene()
        {
            RayTracingScene scene = GetScene();

            IObject3D directionLight = SampleObjectsFactory.GetDirectionLight("DirectionLight", new Vector3(1, 1, 1), 1f);
            directionLight.AddComponent(new TransformRotator(new Vector3(0, 0, 1), 15));

            IObject3D camera = scene.MainCamera.ParentObject;
            camera.Transform.Position = new Vector3(5, 5, 5);
            camera.Transform.DirectAxisByPosition(Vector3.UnitZ, Vector3.Zero);

            IObject3D sphere = SampleObjectsFactory.GetSphere("Sphere", Material.PerfectMirror, 2);
            sphere.Transform.Position = new Vector3(0, 0, 0);

            scene.AddObjects(
                directionLight, 
                sphere
                );

            return scene;
        }

        private static RayTracingScene GetSatelliteScene()
        {
            RayTracingScene scene = GetScene();

            IObject3D directionLight = SampleObjectsFactory.GetDirectionLight("DirectionLight", new Vector3(1, 1, 1), 1f);

            IObject3D camera = scene.MainCamera.ParentObject;
            camera.Transform.Position = new Vector3(0.1f, -5, 0);
            camera.Transform.DirectAxisByPosition(Vector3.UnitZ, Vector3.Zero);

            IObject3D sphere = SampleObjectsFactory.GetSphere("Sphere", Material.PerfectMirror, 2);
            sphere.Transform.Position = new Vector3(0, 0, 0);
            camera.AddComponent(new TransformRotator(true, sphere.Transform, new Vector3(0, 0, 1), 15));

            IObject3D sphere1 = SampleObjectsFactory.GetSphere("Sphere1", Material.PerfectMirror, 0.7f);
            sphere1.Transform.Position = new Vector3(0, -3, 0);

            scene.AddObjects(
                directionLight,
                sphere,
                sphere1
                );

            return scene;
        }

        private static RayTracingScene GetCubeScene()
        {
            RayTracingScene scene = GetScene();

            IObject3D directionLight = SampleObjectsFactory.GetDirectionLight("DirectionLight", new Vector3(0, 0, -1), 1f);
            IObject3D directionLight1 = SampleObjectsFactory.GetDirectionLight("DirectionLight1", new Vector3(0, -1, 0), 0.5f);

            IObject3D camera = scene.MainCamera.ParentObject;
            camera.Transform.Position = new Vector3(0.001f, -5, 5);
            camera.Transform.DirectAxisByPosition(Vector3.UnitZ, Vector3.Zero);

            IObject3D cube = SampleObjectsFactory.GetCube("Cube", Material.PerfectMirror, 2);
            cube.Transform.Position = new Vector3(0, 0, 0);
            camera.AddComponent(new TransformRotator(true, cube.Transform, new Vector3(0, 0, 1), 15));

            scene.AddObjects(
                directionLight,
                directionLight1,
                cube
                );

            return scene;
        }

        private static RayTracingScene GetTwoCubesScene()
        {
            RayTracingScene scene = GetScene();

            IObject3D directionLight = SampleObjectsFactory.GetDirectionLight("DirectionLight", new Vector3(-1, 1, 0), 1f);
            directionLight.AddComponent(new TransformRotator(new Vector3(1, 1, 1), 15));

            IObject3D camera = scene.MainCamera.ParentObject;
            camera.Transform.Position = new Vector3(-5, 6, 0);
            camera.Transform.DirectAxisByPosition(Vector3.UnitZ, new Vector3(2, 0, 0));

            IObject3D cube = SampleObjectsFactory.GetCube("Cube", Material.PerfectMirror, 2);
            cube.Transform.Position = new Vector3(0, 0, 0);

            IObject3D cube1 = SampleObjectsFactory.GetCube("Cube1", Material.PerfectMirror, 2);
            cube1.Transform.Position = new Vector3(2, 6, 0);

            scene.AddObjects(
                directionLight,
                cube,
                cube1
                );

            return scene;
        }
    }
}