using System.Numerics;
using System;
using Hierarchy;
using Engine3D.Components.Abstract;
using Quaternion = MathExtensions.Quaternion;
using Engine3D.Objects;

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
                if (value.X == float.NaN || value.Y == float.NaN || value.Z == float.NaN)
                {
                    //TODO: test it Vector3.One * float.NaN

                    throw new ArgumentException($"Position is invalid; " +
                        $"Position can not be NaN; Value you want to set {value}");
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
                if (value == Vector3.Zero)
                {
                    throw new ArgumentException($"Axis is invalid; " +
                        $"Axis can not be = (0, 0, 0); Value you want to set {value}");
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
                if (value == Vector3.Zero)
                {
                    throw new ArgumentException($"Axis is invalid; " +
                        $"Axis can not be = (0, 0, 0); Value you want to set {value}");
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
                if (value == Vector3.Zero)
                {
                    throw new ArgumentException($"Axis is invalid; " +
                        $"Axis can not be = (0, 0, 0); Value you want to set {value}");
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

            AxisX = new Vector3(1, 0, 0);
            AxisY = new Vector3(0, 1, 0);
            AxisZ = new Vector3(0, 0, 1);
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
    }
}
