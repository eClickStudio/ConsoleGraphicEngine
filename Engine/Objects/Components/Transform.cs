using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Objects.Components
{
    class Transform : Component
    {
        public Vector3 position;
        public Vector3 rotation;

        public Transform() { }

        public Transform(Vector3 position) 
        {
            this.position = position;
        }

        public Transform(Vector3 position, Vector3 rotation) 
        {
            this.position = position;
            this.rotation = rotation;
        }

    }
}
