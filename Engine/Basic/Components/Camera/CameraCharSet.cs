using System;
using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Camera
{
    internal struct CameraCharSet
    {
        public char skyChar { get; }

        private char[] _charsGradient { get; }
        public IReadOnlyList<char> charsGradient => _charsGradient;

        public int charsCount => charsGradient.Count;

        public CameraCharSet(char skyChar, in char[] charsGradient)
        {
            if (charsGradient == null || charsGradient.Length == 0)
            {
                throw new ArgumentException($"Chars gradient is invalid;" +
                    $"It must not be null and has lenght > 0");
            }

            this.skyChar = skyChar;
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

            return $"Sky char =({skyChar}); Gradient = {string.Join(null, charsGradient)})";
        }

        public override bool Equals(object obj)
        {
            return obj is CameraCharSet otherCharSet &&
                   skyChar == otherCharSet.skyChar &&
                   EqualityComparer<char[]>.Default.Equals(_charsGradient, otherCharSet._charsGradient);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(skyChar, _charsGradient);
        }
    }
}
