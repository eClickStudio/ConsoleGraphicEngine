using Engine3D.Components.Abstract;
using Engine3D.Objects;
using System.Numerics;

namespace ConsoleRayTracingRenderer.Scripts
{
    internal class TransformMover : AbstractComponent
    {
        public float Distance { get; private set; }

        private Vector3 _point1;
        public Vector3 Point1
        {
            get => _point1;
            set
            {
                if (_point1 != value)
                {
                    _point1 = value;
                    Distance = Vector3.Distance(Point1, Point2);

                    if (IsMoveToPoint1 && ParentObject != null)
                    {
                        _moveDirection = Vector3.Normalize(Point1 - ParentObject.Transform.Position);
                    }

                    OnChanged();
                }
            }
        }

        private Vector3 _point2;
        public Vector3 Point2
        {
            get => _point2;
            set
            {
                if (_point2 != value)
                {
                    _point2 = value;
                    Distance = Vector3.Distance(Point1, Point2);

                    if (!IsMoveToPoint1 && ParentObject != null)
                    {
                        _moveDirection = Vector3.Normalize(Point2 - ParentObject.Transform.Position);
                    }

                    OnChanged();
                }
            }
        }

        private float _speed;
        public float Speed
        {
            get => _speed;
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    OnChanged();
                }
            }
        }

        private bool _isMoveToPoint1;
        public bool IsMoveToPoint1
        {
            get => _isMoveToPoint1;
            set
            {
                if (_isMoveToPoint1 != value)
                {
                    _isMoveToPoint1 = value;

                    if (value)
                    {
                        _startPosition = Point2;
                    }
                    else
                    {
                        _startPosition = Point1;
                    }

                    if (ParentObject != null)
                    {
                        if (value)
                        {
                            _moveDirection = Vector3.Normalize(Point1 - ParentObject.Transform.Position);
                        }
                        else
                        {
                            _moveDirection = Vector3.Normalize(Point2 - ParentObject.Transform.Position);
                        }
                    }

                    OnChanged();
                }
            }
        }

        private Vector3 _moveDirection;
        private Vector3 _startPosition;

        public TransformMover(Vector3 point1, Vector3 point2, float speed, bool isStartMoveToPoint1 = true)
        {
            Point1 = point1;
            Point2 = point2;

            Speed = speed;

            IsMoveToPoint1 = isStartMoveToPoint1;

            OnAttachedToObjectEvent += UpdateParentObject;
        }

        private void UpdateParentObject(IObject3D parentObject)
        {
            Vector3 position = parentObject.Transform.Position;

            if (position == Point1)
            {
                _startPosition = Point1;
                IsMoveToPoint1 = false;
            }
            else if (position == Point2)
            {
                _startPosition = Point2;
                IsMoveToPoint1 = true;
            }
            else
            {
                if (IsMoveToPoint1)
                {
                    _startPosition = Point2;
                    _moveDirection = Vector3.Normalize(Point1 - parentObject.Transform.Position);
                }
                else
                {
                    _startPosition = Point1;
                    _moveDirection = Vector3.Normalize(Point2 - parentObject.Transform.Position);
                }
            }
        }

        protected override void SubUpdate(uint frameTime)
        {
            Vector3 position = ParentObject.Transform.Position;

            float offset = (float)frameTime / 1000 * Speed;
            Vector3 moveOffset = _moveDirection * offset;

            float progress = Vector3.Distance(_startPosition, position + moveOffset) / Distance;

            if (progress >= 1)
            {
                IsMoveToPoint1 = !IsMoveToPoint1;

                float difference = progress - 1;
                ParentObject.Transform.Position = position + moveOffset + _moveDirection * Distance * difference;
            }
            else
            {
                ParentObject.Transform.Position = position + moveOffset;
            }
        }
    }
}
