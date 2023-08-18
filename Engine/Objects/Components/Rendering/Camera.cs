using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering
{
    class Camera : Component
    {
        public Vector2Int resolution { get; private set; }
        private const float _charAspect = 11f / 24f;
        private float _resolutionAspect;

        private char[] _brightnessGradient;

        private Vector3 DEFAULT_DIRECTION = new Vector3(1, 0, 0);
        //public Vector3 direction
        //{
        //    get
        //    {
        //        return Vector3.Transform(DEFAULT_DIRECTION, new Quaternion(0, ));
        //    }
        //}

        public Camera(Vector2Int resolution, in char[] colorsGradient)
        {
            this.resolution = resolution;
            _resolutionAspect = (float)resolution.X / resolution.Y;

           _brightnessGradient = colorsGradient;
        }

        public char GetPixel(float brightness)
        {
            brightness = Math.Clamp(brightness, 0, 1);

            int brightnessIndex = Math.Clamp((int)(brightness * _brightnessGradient.Length), 0, _brightnessGradient.Length - 1); ;

            return _brightnessGradient[brightnessIndex];
        }

        public Vector2 GetRelativePosition(int absoluteX, int absoluteY)
        {
            return new Vector2(
                ((float)absoluteX / resolution.X * 2 - 1) * _resolutionAspect * _charAspect,
                (float)absoluteY / resolution.Y * 2 - 1
            );
        }

        public Vector2 GetRelativePosition(Vector2Int absolutePosition)
        {
            return GetRelativePosition(absolutePosition.X, absolutePosition.Y);
        }

        //public Vector3 GetRayDirection(Vector2Int screenPosition)
        //{

        //}
    }
}
