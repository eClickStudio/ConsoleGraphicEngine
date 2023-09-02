using System.Collections.Generic;

namespace ConsoleGraphicEngine.Engine.Objects.Components.Rendering
{
    internal struct CameraCharSet
    {
        public char skyChar { get; }

        private char[] _brightnessGradient { get; }
        public IReadOnlyList<char> brightnessGradient => _brightnessGradient;

        public int charCount => brightnessGradient.Count;

        public CameraCharSet(char skyChar, in char[] brightnessGradient)
        {
            this.skyChar= skyChar;
            _brightnessGradient = brightnessGradient;
        }
    }
}
