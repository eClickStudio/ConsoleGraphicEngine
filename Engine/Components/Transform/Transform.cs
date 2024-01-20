using System.Numerics;
using System;
using Hierarchy;
using Engine3D.Components.Abstract;
using Quaternion = MathExtensions.Quaternion;
using Engine3D.Objects;
using MathExtensions;

namespace Engine3D.Components.Transform
{
    public class Transform : AbstractComponent, ITransform
    {
        private Vector3 _position;

        public Vector3 Position { 
            get
            {
                return _position;
            }
            set
            {
                if (!value.IsNormal())
                {
                    throw new ArgumentException($"Position is invalid;\n" +
                        $"Value you want to set {value};");
                }

                if (_position != value)
                {
                    _position = value;

                    if (Hierarchy.Parent != null)
                    {
                        _localPosition = Hierarchy.Parent.Position - value;
                    }
                    else
                    {
                        _localPosition = value;
                    }

                    UpdateChildrenPosition();

                    onPositionChangedEvent?.Invoke();
                    OnChanged();
                }
            }
        }

        private Vector3 _localPosition;
        public Vector3 LocalPosition 
        {
            get
            {
                return _localPosition;
            }
            set
            {
                if (!value.IsNormal())
                {
                    throw new ArgumentException($"Local position is invalid;\n" +
                        $"Value you want to set {value};");
                }

                if (_localPosition != value)
                {
                    _localPosition = value;
                    UpdatePosition();
                }
            }
        }

        private void UpdateChildrenPosition()
        {
            foreach (ITransform transform in Hierarchy.Children)
            {
                transform.UpdatePosition();
            }
        }

        public void UpdatePosition()
        {
            if (Hierarchy.Parent != null)
            {
                Position = Hierarchy.Parent.Position + LocalPosition;
            }
            else
            {
                Position = LocalPosition;
            }
        }

        private Vector3 _axisX;
        public Vector3 AxisX 
        {
            get
            {
                return _axisX;
            }
            private set
            {
                if (value == Vector3.Zero || !value.IsNormal())
                {
                    throw new ArgumentException($"Axis is invalid;\n" +
                        $"Value you want to set {value};");
                }

                value = Vector3.Normalize(value);

                if (_axisX != value) 
                { 
                    _axisX = value;
                }
            } 
        }

        private Vector3 _axisY;
        public Vector3 AxisY
        {
            get
            {
                return _axisY;
            }
            private set
            {
                if (value == Vector3.Zero || !value.IsNormal())
                {
                    throw new ArgumentException($"Axis is invalid;\n" +
                        $"Value you want to set {value};");
                }

                value = Vector3.Normalize(value);

                if (_axisY != value)
                {
                    _axisY = value;
                }
            }
        }

        private Vector3 _axisZ;
        public Vector3 AxisZ
        {
            get
            {
                return _axisZ;
            }
            private set
            {
                if (value == Vector3.Zero || !value.IsNormal())
                {
                    throw new ArgumentException($"Axis is invalid;\n" +
                        $"Value you want to set {value};");
                }

                value = Vector3.Normalize(value);

                if (_axisZ != value)
                {
                    _axisZ = value;
                }
            }
        }

        public string HierarchyName { get; set; }

        public IHierarchyManager<ITransform> Hierarchy { get; }


        public event Action onPositionChangedEvent;
        public event Action OnRotationChangedEvent;


        public Transform(ITransform parent = null)
        {
            Hierarchy = new HierarchyManager<ITransform>(this, parent);
            Hierarchy.OnHierarchyChangedEvent += () => OnChanged();

            OnAttachedToObjectEvent += ChangeHierarchyName;

            AxisX = Vector3.UnitX;
            AxisY = Vector3.UnitY;
            AxisZ = Vector3.UnitZ;
        }

        private void ChangeHierarchyName(IObject3D object3D)
        {
            if (string.IsNullOrEmpty(HierarchyName))
            {
                HierarchyName = object3D.Name;
            }
        }

        //TODO: test rotations and hierarchy

        public void RotateAroundAxis(Vector3 rotationAxis, float angle)
        {
            Vector3 rotatedAxisX = Vector3.Normalize(Quaternion.RotateVector(AxisX, rotationAxis, angle));
            Vector3 rotatedAxisY = Vector3.Normalize(Quaternion.RotateVector(AxisY, rotationAxis, angle));
            Vector3 rotatedAxisZ = Vector3.Normalize(Quaternion.RotateVector(AxisZ, rotationAxis, angle));

            if (rotatedAxisX != AxisX || rotatedAxisY != AxisY || rotatedAxisZ != AxisZ)
            {
                foreach (ITransform transform in Hierarchy.Children)
                {
                    transform.SynchronousRotateAroundPoint(Position, rotationAxis, angle);
                }

                AxisX = rotatedAxisX;
                AxisY = rotatedAxisY;
                AxisZ = rotatedAxisZ;

                OnRotationChangedEvent?.Invoke();
                OnChanged();
            }
        }

        public void RotateAroundPoint(Vector3 point, Vector3 rotationAxis, float angle)
        {
            Vector3 offset = Position - point;
            Vector3 rotatedOffset = Quaternion.RotateVector(offset, rotationAxis, angle);

            Position = point + rotatedOffset;

            foreach (ITransform transform in Hierarchy.Children)
            {
                transform.SynchronousRotateAroundPoint(Position, rotationAxis, angle);
            }

        }

        public void SynchronousRotateAroundPoint(Vector3 point, Vector3 rotationAxis, float angle)
        {
            RotateAroundPoint(point, rotationAxis, angle);
            RotateAroundAxis(rotationAxis, angle);
        }

        public void DirectAxisByVector(Vector3 localAxis, Vector3 directionVector)
        {
            if (!VectorExtension.IsNormal(localAxis) && localAxis.Length() != 0)
            {
                throw new ArgumentException($"LocalAxis is invalid; localAxis = {localAxis}");
            }
            if (!VectorExtension.IsNormal(directionVector) && directionVector.Length() != 0)
            {
                throw new ArgumentException($"DirectionVector is invalid; directionVector = {directionVector}");
            }

            localAxis = Vector3.Normalize(localAxis);
            directionVector = Vector3.Normalize(directionVector);

            float angle = VectorExtension.GetAngleBetweenVectors(localAxis, directionVector);
            Vector3 cross = Vector3.Cross(directionVector, localAxis);

            if (!cross.IsNormal())
            {
                throw new ArithmeticException($"Cross is invalid; Cross of {directionVector} and {localAxis} = {cross}");
            }

            if (cross.Length() == 0)
            {
                //directionVeector and localAxis are parallel => rotationAxis = perpendicular vector for dirVec ans locAxis

                if (directionVector == -localAxis)
                {
                    AxisX = -AxisX;
                    AxisY = -AxisY;
                    AxisZ = -AxisZ;

                    OnRotationChangedEvent?.Invoke();
                    OnChanged();
                }
                else
                {
                    throw new ArithmeticException($"Can't direct axis by vector; vector = {directionVector}; axis = {localAxis}");
                }
            }
            else
            {
                Vector3 rotationAxis = VectorExtension.GetRotationAxis(localAxis, directionVector, angle);

                RotateAroundAxis(rotationAxis, angle);
            }
        }

        public void DirectAxisByPosition(Vector3 localAxis, Vector3 worldPoint)
        {
            Vector3 directionVector = Vector3.Normalize(worldPoint - Position);

            DirectAxisByVector(localAxis, directionVector);
        }

        /// <summary>
        /// Converts Position from local to world
        /// </summary>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public Vector3 ConvertPositionFromLocalToWorld(Vector3 localPosition)
        {
            Vector3 worldVector = ConvertVectorFromLocalToWorld(AxisX, AxisY, AxisZ, localPosition);

            return Position + worldVector;
        }

        /// <summary>
        /// Converts Position from world to local
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector3 ConvertPositionFromWorldToLocal(Vector3 worldPosition)
        {
            Vector3 localVector = ConvertVectorFromWorldToLocal(AxisX, AxisY, AxisZ, worldPosition);

            return localVector - Position;
        }

        /// <summary>
        /// Converts vector from local space to world space (If local space does't rotate ralative world space than local and world vectors are identity)
        /// </summary>
        /// <param name="localAxisX">Local AxisX ralative world space</param>
        /// <param name="localAxisY">Local AxisY ralative world space</param>
        /// <param name="localAxisZ">Local AxisZ ralative world space</param>
        /// <param name="localVector">Local Vector you want to convert</param>
        /// <returns></returns>
        public static Vector3 ConvertVectorFromLocalToWorld(Vector3 localAxisX, Vector3 localAxisY, Vector3 localAxisZ, Vector3 localVector)
        {
            return localVector.X * localAxisX + localVector.Y * localAxisY + localVector.Z * localAxisZ;
        }

        public Vector3 ConvertVectorFromLocalToWorld(Vector3 localVector)
        {
            return ConvertVectorFromLocalToWorld(AxisX, AxisY, AxisZ, localVector);
        }

        /// <summary>
        /// Converts vector from world space to local space (If local space does't rotate ralative world space than local and world vectors are identity)
        /// </summary>
        /// <param name="localAxisX">Local AxisX ralative world space</param>
        /// <param name="localAxisY">Local AxisY ralative world space</param>
        /// <param name="localAxisZ">Local AxisZ ralative world space</param>
        /// <param name="worldVector">World Vector you want to convert</param>
        /// <returns></returns>
        public static Vector3 ConvertVectorFromWorldToLocal(Vector3 localAxisX, Vector3 localAxisY, Vector3 localAxisZ, Vector3 worldVector)
        {
            //TODO: realize method
            throw new NotImplementedException();
        }

        public Vector3 ConvertVectorFromWorldToLocal(Vector3 localVector)
        {
            return ConvertVectorFromWorldToLocal(AxisX, AxisY, AxisZ, localVector);
        }
    }
}
