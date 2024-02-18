using RayTracingGraphicEngine3D.RayTracingEngine.Tools;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;
using MathExtensions;
using RayTracingGraphicEngine3D.RayTracingEngine.Components.Rendering;
using RayTracingGraphicEngine3D.RayTracingEngine.Rays;

namespace RayTracingGraphicEngine3D.RayTracingEngine.Components.Camera.Abstract
{
    public abstract class AbstractRayTracingCamera : AbstractComponent, IRayTracingCamera
    {
        private Material? _sceneEnvironmentMaterial;
        public Material? SceneEnvironmentMaterial
        {
            get => _sceneEnvironmentMaterial;
            set
            {
                if (_sceneEnvironmentMaterial != value)
                {
                    _sceneEnvironmentMaterial = value;

                    OnChanged();
                }
            }
        }
        private Material? _environmentMaterial;

        protected float ResolutionAspect { get; private set; }
        public Vector2Int Resolution { get; }

        /// <summary>
        /// Char X size divided by Char Y size; X / Y
        /// </summary>
        protected float CharAspect { get; }

        private CameraCharSet _charSet;
        public CameraCharSet CharSet
        {
            get
            {
                return _charSet;
            }
            set
            {
                if (value != _charSet)
                {
                    _charSet = value;

                    OnChanged();
                }
            }
        }


        public AbstractRayTracingCamera(Vector2Int resolution, Vector2Int charSize, CameraCharSet charSet)
        {
            if (resolution <= Vector2Int.Zero)
            {
                throw new ArgumentException($"Camera resolution is invalid; cameraResolution = {resolution};" +
                    $"It must be > (0; 0)");
            }
            if (charSize <= Vector2Int.Zero)
            {
                throw new ArgumentException($"Char size is invalid; charSize = {charSize};" +
                    $"It must be > (0; 0)");
            }

            Resolution = resolution;
            ResolutionAspect = Resolution.X / Resolution.Y;

            CharAspect = (float)charSize.X / charSize.Y;
            CharSet = charSet;
        }

        public LightRay GetRay(Vector2Int screenPosition)
        {
            if (screenPosition < Vector2Int.Zero || screenPosition >= Resolution)
            {
                throw new ArgumentException($"Screen position is invalid; Min = (0, 0, 0); Max = {Resolution}; " +
                    $"Your argument = {screenPosition}");
            }

            if (!_environmentMaterial.HasValue)
            {
                _environmentMaterial = GetEnvironmentMaterial();
            }

            return new LightRay(CreateRay(GetRelativePosition(screenPosition)), 1, _environmentMaterial.Value, 0, "camera", null, "camera");
        }

        protected abstract Ray CreateRay(Vector2 relativePosition);

        public char GetChar(float? intensity)
        {
            if (intensity.HasValue)
            {
                intensity = MathExtension.Clamp(intensity.Value, 0, 1);
                int colorIndex = MathExtension.Clamp((int)(intensity * CharSet.CharsCount), 0, CharSet.CharsCount - 1); ;

                return CharSet.CharsGradient[colorIndex];
            }
            else
            {
                //TODO: replace to skybox
                return CharSet.SkyChar;
            }
        }

        private Vector2 GetRelativePosition(Vector2Int screenPosition)
        {
            float x = (float)screenPosition.X / Resolution.X * 2 - 1;
            float y = 1 - (float)screenPosition.Y / Resolution.Y * 2;

            y *= ResolutionAspect * CharAspect;

            //Console.WriteLine($"Relative position = {new Vector2(x, y)}");

            return new Vector2(x, y);

            ////Before:
            //float x = (float)screenPosition.X / Resolution.X * 2 - 1;
            //float y = 1 - (float)screenPosition.Y / Resolution.Y * 2;

            //y *= ResolutionAspect * CharAspect;

            //Console.WriteLine($"Relative position = {new Vector2(x, y)}");

            //return new Vector2(x, y);
        }

        protected override void SubUpdate(uint frameTime)
        {
            _environmentMaterial = GetEnvironmentMaterial();
        }

        private Material GetEnvironmentMaterial()
        {
            //TODO: return real Environment material

            if (SceneEnvironmentMaterial.HasValue)
            {
                return SceneEnvironmentMaterial.Value;
            }
            else
            {
                throw new InvalidOperationException("Camera sceneEnvironmentMaterial haven't initialized yet!");
            }
        }
    }
}
