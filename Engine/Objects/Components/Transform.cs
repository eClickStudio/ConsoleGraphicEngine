using System;
using System.Numerics;
using ConsoleGraphicEngine.Engine.Objects.Components.Abstract;
using Quaternion = ConsoleGraphicEngine.Engine.Tools.Quaternion;

namespace ConsoleGraphicEngine.Engine.Objects.Components
{
    class Transform : Component
    {
        public Vector3 position;

        public Vector3 axisX { get; private set; } = new Vector3(1, 0, 0);
        public Vector3 axisY { get; private set; } = new Vector3(0, 1, 0);
        public Vector3 axisZ { get; private set; } = new Vector3(0, 0, 1);

        public Transform() 
        {

        }

        public Transform(Vector3 position) 
        {
            this.position = position;
        }

        public void Rotate(Vector3 axis, float angle)
        {
            axisZ = Vector3.Normalize(RotateVector(axisZ, axis, angle));
            axisX = Vector3.Normalize(RotateVector(axisX, axis, angle));
            axisY = Vector3.Normalize(RotateVector(axisY, axis, angle));
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
