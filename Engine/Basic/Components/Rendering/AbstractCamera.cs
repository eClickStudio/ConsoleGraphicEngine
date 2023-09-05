﻿using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;
using ConsoleGraphicEngine.Engine.Basic.Tools;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Rendering
{
    internal class AbstractCamera : AbstractComponent, ICamera
    {
        public Vector2Int resolution { get; }

        /// <summary>
        /// Char X size divided by Char Y size; X / Y
        /// </summary>
        protected float charAspect { get; }

        private float _charsPerUnit;
        public float charsPerUnit 
        {
            get
            {
                return _charsPerUnit;
            }
            set
            {
                if (_charsPerUnit != value)
                {
                    _charsPerUnit = value;

                    OnChanged();
                }
            }
        }

        public Vector2 screenWorldSize => resolution / charsPerUnit;


        private Vector2 MIN_CAMERA_ANGLE { get; } = Vector2.Zero;
        private Vector2 MAX_CAMERA_ANGLE { get; } = Vector2.One * (float)(Math.PI * 2);

        private Vector2 _cameraAngle;
        public Vector2 cameraAngle 
        {
            get
            {
                return _cameraAngle;
            } 
            set
            {
                if (_cameraAngle != value 
                    && value == Vector2.Clamp(value, MIN_CAMERA_ANGLE, MAX_CAMERA_ANGLE))
                {
                    _cameraAngle = value;

                    OnChanged();
                }
            } 
        }

        private CameraCharSet _charSet;
        public CameraCharSet charSet 
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


        public AbstractCamera(Vector2Int resolution, Vector2Int charSize, Vector2 cameraAngle, float charsPerUnit, CameraCharSet charSet)
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
            if (charsPerUnit <= 0)
            {
                throw new ArgumentException($"Chars per unit is invalid; charsPerUnit = {charsPerUnit};" +
                    $"It must be > {MIN_CAMERA_ANGLE} and < {MAX_CAMERA_ANGLE}");
            }

            this.resolution = resolution;
            charAspect = (float)charSize.X / charSize.Y;

            this.cameraAngle = cameraAngle;

            this.charsPerUnit = charsPerUnit;
            this.charSet = charSet;
        }

        public char GetChar(float? brightness)
        {
            if (brightness.HasValue)
            {
                brightness = Math.Clamp(brightness.Value, 0, 1);

                int brightnessIndex = Math.Clamp((int)(brightness * charSet.charsCount), 0, charSet.charsCount - 1); ;

                return charSet.charsGradient[brightnessIndex];
            }
            else
            {
                return charSet.skyChar;
            }
        }

        public Vector2 GetRelativePosition(int absoluteX, int absoluteY)
        {
            return new Vector2(
                ((float)absoluteX / resolution.X * 2 - 1) * charAspect,
                1 - (float)absoluteY / resolution.Y * 2
            );
        }
    }
}
