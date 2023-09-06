using System.Numerics;
using System;
using ConsoleGraphicEngine.Engine.Basic.Tools.Hierarchy;
using ConsoleGraphicEngine.Engine.Basic.Components.Abstract;
using Quaternion = ConsoleGraphicEngine.Engine.Basic.Tools.Quaternion;

namespace ConsoleGraphicEngine.Engine.Basic.Components.Transform
{
    internal class Transform : AbstractComponent, ITransform
    {
        private Vector3 _position;

        public Vector3 position { 
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

                    UpdateChildrenPosition();

                    onPositionChanged?.Invoke();
                    OnChanged();
                }
            }
        }

        private Vector3 _localPosition;
        public Vector3 localPosition 
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
                    UpdateLocalPosition();
                }
            }
        }

        private void UpdateChildrenPosition()
        {
            foreach (ITransform transform in hierarchy.children)
            {
                transform.UpdateLocalPosition();
            }
        }

        public void UpdateLocalPosition()
        {
            position = hierarchy.parent.position + localPosition;
        }

        private Vector3 _axisX;
        public Vector3 axisX 
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

                    onRotationChanged?.Invoke();
                    OnChanged();
                }
            } 
        }

        private Vector3 _axisY;
        public Vector3 axisY
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

                    onRotationChanged?.Invoke();
                    OnChanged();
                }
            }
        }

        private Vector3 _axisZ;
        public Vector3 axisZ
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

                    onRotationChanged?.Invoke();
                    OnChanged();
                }
            }
        }

        public IHierarchyManager<ITransform> hierarchy { get; }

        public event Action onPositionChanged;
        public event Action onRotationChanged;


        public Transform(ITransform parent)
        {
            hierarchy = new HierarchyManager<ITransform>(this, parent);
            hierarchy.onHierarchyChanged += () => OnChanged();

            axisX = new Vector3(1, 0, 0);
            axisY = new Vector3(0, 1, 0);
            axisZ = new Vector3(0, 0, 1);
        }


        public void RotateAroundAxis(Vector3 rotationAxis, float angle)
        {
            axisX = Vector3.Normalize(Quaternion.RotateVector(axisX, rotationAxis, angle));
            axisY = Vector3.Normalize(Quaternion.RotateVector(axisY, rotationAxis, angle));
            axisZ = Vector3.Normalize(Quaternion.RotateVector(axisZ, rotationAxis, angle));
        }

        public void RotateAroundPoint(Vector3 point, Vector3 rotationAxis, float angle)
        {
            Vector3 offset = position - point;
            Vector3 rotatedOffset = Quaternion.RotateVector(offset, rotationAxis, angle);

            position = point + rotatedOffset;
        }

        public void SynchronousRotateAroundPoint(Vector3 point, Vector3 rotationAxis, float angle)
        {
            RotateAroundPoint(point, rotationAxis, angle);
            RotateAroundAxis(rotationAxis, angle);
        }
    }
}
