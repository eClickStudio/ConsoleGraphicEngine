using Engine3D.Components.Abstract;
using Engine3D.Components.Transform;
using Engine3D.Objects;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Light.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering.Abstract;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays.IntersectableShapes;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera;
using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using System.Numerics;
using System.Runtime.InteropServices;

namespace RayTracingGraphicEngine3D.Samples
{
    public static class SampleObjectsFactory
    {
        public static IObject3D CreateObject(string name, params IComponent[] components)
        {
            Object3D object3D = new Object3D(name);
            object3D.AddComponents(components);

            return object3D;
        }

        #region Rendering
        public static IObject3D GetFishEyeCamera(string name, Vector2Int resolution, Vector2Int charSize, Vector2 cameraAngle, CameraCharSet charSet)
        {
            return CreateObject(name,
                new FishEyeCamera(resolution, charSize, cameraAngle, charSet));
        }

        public static IObject3D GetOrthogonalCamera(string name, Vector2Int resolution, Vector2Int charSize, Vector2 cameraSize, CameraCharSet charSet)
        {
            return CreateObject(name,
                new OrthogonalCamera(resolution, charSize, cameraSize, charSet));
        }

        public static IObject3D GetDirectionLight(string name, Vector3 direction, float intensity)
        {
            return CreateObject(name,
                new DirectionLight(direction, intensity));
        }
        #endregion

        #region Light
        private static IObject3D GetShapeLightObject(in ITransform transform, in IIntersectableShape shape, string name, float intensity)
        {
            Object3D object3D = new Object3D(transform, name);
            ShapedLight shapedLight = new ShapedLight(shape, intensity);
            object3D.AddComponent(shapedLight);

            return object3D;
        }

        public static IObject3D GetSphereLight(string name, float intensity, float radius)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new SphereShape(transform, radius);

            return GetShapeLightObject(transform, shape, name, intensity);
        }

        public static IObject3D GetBoxLight(string name, float intensity, Vector3 size)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new BoxShape(transform, size);

            return GetShapeLightObject(transform, shape, name, intensity);
        }

        public static IObject3D GetCudeLight(string name, float intensity, float edgeLenght)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new CubeShape(transform, edgeLenght);

            return GetShapeLightObject(transform, shape, name, intensity);
        }

        public static IObject3D GetPlaneLight(string name, float intensity, Vector3 normalVector, float offset)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new PlaneShape(transform, normalVector, offset);

            return GetShapeLightObject(transform, shape, name, intensity);
        }
        #endregion

        #region ShapeRenderers
        private static IObject3D GetShapeRendererObject(in ITransform transform, in IIntersectableShape shape, string name, Material material)
        {
            Object3D object3D = new Object3D(transform, name);
            IShapeRenderer shapeRenderer = new ShapeRenderer(shape, material);
            object3D.AddComponent(shapeRenderer);

            return object3D;
        }

        public static IObject3D GetSphere(string name, Material material, float radius)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new SphereShape(transform, radius);

            return GetShapeRendererObject(transform, shape, name, material);
        }

        public static IObject3D GetCube(string name, Material material, float edgeLenght)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new CubeShape(transform, edgeLenght);

            return GetShapeRendererObject(transform, shape, name, material);
        }

        public static IObject3D GetBox(string name, Material material, Vector3 size)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new BoxShape(transform, size);

            return GetShapeRendererObject(transform, shape, name, material);
        }

        public static IObject3D GetPlane(string name, Material material, Vector3 normalVector, float offset)
        {
            ITransform transform = new Transform();
            IIntersectableShape shape = new PlaneShape(transform, normalVector, offset);

            return GetShapeRendererObject(transform, shape, name, material);
        }
        #endregion
    }
}
