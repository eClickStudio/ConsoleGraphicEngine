using RayTracingGraphicEngine3D.Tools;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;
using MathExtensions;

namespace RayTracingGraphicEngine3D.Components.Camera.Abstract
{
    public class AbstractCamera : AbstractComponent, ICamera
    {
        public Vector2Int Resolution { get; }

        /// <summary>
        /// Char X size divided by Char Y size; X / Y
        /// </summary>
        protected float CharAspect { get; }

        private Vector2 MIN_CAMERA_ANGLE { get; } = Vector2.Zero;
        private Vector2 MAX_CAMERA_ANGLE { get; } = Vector2.One * (float)(Math.PI * 2);

        private Vector2 _cameraAngle;
        public Vector2 CameraAngle
        {
            get
            {
                return _cameraAngle;
            }
            set
            {
                if (!value.IsNormal() || value != Vector2.Clamp(value, MIN_CAMERA_ANGLE, MAX_CAMERA_ANGLE))
                {
                    throw new ArgumentException($"Camera angle is invalid; " +
                        $"Min = {MIN_CAMERA_ANGLE}; Max = {MAX_CAMERA_ANGLE}; Value you want to set {value}");
                }

                if (_cameraAngle != value)
                {
                    _cameraAngle = value;

                    OnChanged();
                }
            }
        }

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


        public AbstractCamera(Vector2Int resolution, Vector2Int charSize, Vector2 cameraAngle, CameraCharSet charSet)
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
            if (cameraAngle != Vector2.Clamp(cameraAngle, MIN_CAMERA_ANGLE, MAX_CAMERA_ANGLE))
            {
                throw new ArgumentException($"Camera angle is invalid; cameraAngle = {cameraAngle};" +
                    $"It must be > {MIN_CAMERA_ANGLE} and < {MAX_CAMERA_ANGLE}");
            }
            //if (charsPerUnit <= 0)
            //{
            //    throw new ArgumentException($"Chars per unit is invalid; charsPerUnit = {charsPerUnit};" +
            //        $"It must be > {MIN_CAMERA_ANGLE} and < {MAX_CAMERA_ANGLE}");
            //}

            Resolution = resolution;
            CharAspect = (float)charSize.X / charSize.Y;

            CameraAngle = cameraAngle;

            CharSet = charSet;
        }

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

        public Vector2 GetRelativePosition(int absoluteX, int absoluteY)
        {
            float aspect = Resolution.X / Resolution.Y;

            float x = (float)absoluteX / Resolution.X * 2 - 1;
            float y = 1 - (float)absoluteY / Resolution.Y * 2;

            y *= aspect * CharAspect;

            return new Vector2(x, y);
        }
    }
}
