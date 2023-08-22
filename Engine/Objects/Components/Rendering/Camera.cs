using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering
{
    class Camera : Component
    {
        public Vector2Int resolution { get; private set; }
        private const float _charAspect = 11f / 24f;
        private float _resolutionAspect { get; }

        private char[] _brightnessGradient;

        public float pixelsPerUnit { get; }
        private Vector2 _screenWorldSize { get; }

        public Camera(Vector2Int resolution, float pixelsPerUnit, in char[] colorsGradient)
        {
            this.resolution = resolution;
            this.pixelsPerUnit = pixelsPerUnit;
            _resolutionAspect = (float)resolution.X / resolution.Y;

           _brightnessGradient = colorsGradient;

            _screenWorldSize = resolution / pixelsPerUnit;
        }

        public char GetPixel(float brightness)
        {
            brightness = Math.Clamp(brightness, 0, 1);

            int brightnessIndex = Math.Clamp((int)(brightness * _brightnessGradient.Length), 0, _brightnessGradient.Length - 1); ;

            return _brightnessGradient[brightnessIndex];
        }

        private Vector2 GetRelativePosition(int absoluteX, int absoluteY)
        {
            return new Vector2(
                ((float)absoluteX / resolution.X * 2 - 1) * _resolutionAspect * _charAspect,
                (float)absoluteY / resolution.Y * 2 - 1
            );
        }

        public Vector3 GetRayDirection(Vector2Int screenPosition)
        {
            Transform transform = parentObject.transform;

            Vector2 relativeScreenPosition = GetRelativePosition(screenPosition.X, screenPosition.Y);

            float x = relativeScreenPosition.X * _screenWorldSize.X;
            float y = relativeScreenPosition.Y * _screenWorldSize.Y;

            return Vector3.Normalize(x * transform.directionRight + y * transform.directionUp + transform.directionForward);
        }
    }
}
