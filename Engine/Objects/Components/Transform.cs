using System;
using System.Numerics;
using ConsoleGraphicEngine.Engine.Tools;
using Quaternion = ConsoleGraphicEngine.Engine.Tools.Quaternion;

namespace ConsoleGraphicEngine.Engine.Objects.Components
{
    class Transform : Component
    {
        public Vector3 position;

        public Vector3 directionUp { get; private set; } = new Vector3(0, 0, 1);
        public Vector3 directionForward { get; private set; } = new Vector3(1, 0, 0);
        public Vector3 directionRight { get; private set; } = new Vector3(0, 1, 0);

        public Transform() 
        {

        }

        public Transform(Vector3 position) 
        {
            this.position = position;
        }

        public void Rotate(Vector3 axis, float angle)
        {
            directionForward = Vector3.Normalize(RotateVector(directionForward, axis, angle));
            directionRight = Vector3.Normalize(RotateVector(directionRight, axis, angle));
            directionUp = Vector3.Normalize(RotateVector(directionUp, axis, angle));
        }

        private Vector3 RotateVector(Vector3 vector, Vector3 axis, float angle)
        {
            if (axis == Vector3.Zero)
            {
                throw new ArgumentException("Axis vector cannot be null!");
            }

            axis = Vector3.Normalize(axis);

            Quaternion directionQuaternion = new Quaternion(0, vector.X, vector.Y, vector.Z);

            float cos = (float)Math.Cos(angle / 2);
            float sin = (float)Math.Sin(angle / 2);
            Quaternion axisQuaternion = new Quaternion(cos, sin * axis.X, sin * axis.Y, sin * axis.Z);

            Quaternion resultQuaternion = axisQuaternion * directionQuaternion * Quaternion.Сonjugate(axisQuaternion);

            return new Vector3(resultQuaternion.b, resultQuaternion.c, resultQuaternion.d);
        }
    }
}
