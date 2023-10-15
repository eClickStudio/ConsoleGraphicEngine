using System;
using System.Collections.Generic;

namespace RayTracingGraphicEngine3D.Components.Camera
{
    public struct CameraCharSet
    {
        public char SkyChar { get; }

        private readonly char[] _charsGradient;
        public IReadOnlyList<char> CharsGradient => _charsGradient;

        public int CharsCount => CharsGradient.Count;

        public CameraCharSet(char skyChar, in char[] charsGradient)
        {
            if (charsGradient == null || charsGradient.Length == 0)
            {
                throw new ArgumentException($"Chars gradient is invalid;" +
                    $"It must not be null and has lenght > 0");
            }

            SkyChar = skyChar;
            _charsGradient = charsGradient;
        }

        public static bool operator ==(CameraCharSet a, CameraCharSet b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CameraCharSet a, CameraCharSet b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            //TODO: test this

            return $"Sky char =({SkyChar}); Gradient = {string.Join(null, CharsGradient)})";
        }

        public override bool Equals(object obj)
        {
            return obj is CameraCharSet otherCharSet &&
                   SkyChar == otherCharSet.SkyChar &&
                   EqualityComparer<char[]>.Default.Equals(_charsGradient, otherCharSet._charsGradient);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SkyChar, _charsGradient);
        }
    }
}
