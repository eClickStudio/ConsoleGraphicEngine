using ConsoleGraphicEngine3D.Engine.Basic.Tools;
using Engine3D.Components.Abstract;
using System;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.Basic.Components.Camera
{
    public class AbstractCamera : AbstractComponent, ICamera
    {
        public Vector2Int Resolution { get; }

        /// <summary>
        /// Char X size divided by Char Y size; X / Y
        /// </summary>
        protected float CharAspect { get; }

        //private float _charsPerUnit;
        //public float charsPerUnit 
        //{
        //    get
        //    {
        //        return _charsPerUnit;
        //    }
        //    set
        //    {
        //        if (value < 1)
        //        {
        //            throw new ArgumentException($"Chars per unit is invalid; " +
        //                $"Value can not be < 1; Value you want to set {value}");
        //        }

        //        if (_charsPerUnit != value)
        //        {
        //            _charsPerUnit = value;

        //            OnChanged();
        //        }
        //    }
        //}


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
                if (value != Vector2.Clamp(value, MIN_CAMERA_ANGLE, MAX_CAMERA_ANGLE))
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

            this.Resolution = resolution;
            CharAspect = (float)charSize.X / charSize.Y;

            this.CameraAngle = cameraAngle;

            this.CharSet = charSet;
        }

        public char GetChar(float? brightness)
        {
            if (brightness.HasValue)
            {
                brightness = Math.Clamp(brightness.Value, 0, 1);

                int brightnessIndex = Math.Clamp((int)(brightness * CharSet.CharsCount), 0, CharSet.CharsCount - 1); ;

                return CharSet.CharsGradient[brightnessIndex];
            }
            else
            {
                return CharSet.SkyChar;
            }
        }

        public Vector2 GetRelativePosition(int absoluteX, int absoluteY)
        {
            return new Vector2(
                ((float)absoluteX / Resolution.X * 2 - 1) * CharAspect,
                1 - (float)absoluteY / Resolution.Y * 2
            );
        }
    }
}
