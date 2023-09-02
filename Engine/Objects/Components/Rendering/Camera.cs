using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using ConsoleGraphicEngine.Engine.Tools;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering
{
    class Camera : Component
    {
        public Vector2Int resolution { get; }
        private float _charAspect { get; }

        private CameraCharSet _charSet { get; }

        public float pixelsPerUnit { get; }
        private Vector2 _screenWorldSize { get; }
        private float _cameraDepth { get; }

        /// <summary>
        /// Constructor of camera
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="charSize">Size of char in pixels</param>
        /// <param name="pixelsPerUnit">How much pixels must be in world unit</param>
        /// <param name="cameraDepth"></param>
        /// <param name="colorsGradient"></param>
        public Camera(Vector2Int resolution, Vector2Int charSize, float pixelsPerUnit, float cameraDepth, CameraCharSet charSet)
        {
            this.resolution = resolution;
            _charAspect = (float)charSize.X / charSize.Y;

            _charSet = charSet;

            this.pixelsPerUnit = pixelsPerUnit;
            _screenWorldSize = resolution / pixelsPerUnit;
            _cameraDepth = _screenWorldSize.X * cameraDepth;
        }

        public char GetPixel(float? brightness)
        {
            if (brightness.HasValue)
            {
                brightness = Math.Clamp(brightness.Value, 0, 1);

                int brightnessIndex = Math.Clamp((int)(brightness * _charSet.charCount), 0, _charSet.charCount - 1); ;

                return _charSet.brightnessGradient[brightnessIndex];
            }
            else
            {
                return _charSet.skyChar;
            }            
        }

        private Vector2 GetRelativePosition(int absoluteX, int absoluteY)
        {
            return new Vector2(
                ((float)absoluteX / resolution.X * 2 - 1) * _charAspect,
                (1 - (float)absoluteY / resolution.Y * 2)
            );
        }

        public Vector3 GetRayDirection(Vector2Int screenPosition)
        {
            Transform transform = parentObject.transform;

            Vector2 relativeScreenPosition = GetRelativePosition(screenPosition.X, screenPosition.Y);

            float offsetX = relativeScreenPosition.X * _screenWorldSize.X / 2;
            float offsetY = relativeScreenPosition.Y * _screenWorldSize.Y / 2;

            return Vector3.Normalize(offsetX * transform.axisX + offsetY * transform.axisY + _cameraDepth * transform.axisZ);
        }
    }
}
