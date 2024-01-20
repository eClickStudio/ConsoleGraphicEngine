using System.Numerics;
using Engine3D.Components.Transform;
using Engine3D.Components.Abstract;

namespace ConsoleRayTracingRenderer.Scripts
{
    internal class TransformRotator : AbstractComponent
    {
        internal enum RotationType : byte
        {
            AroundAxis,
            AroundPoint,
            SynchronousAroundPoint
        }

        private RotationType _rotationScheme;
        public RotationType RotationScheme
        {
            get => _rotationScheme;
            set
            {
                if (_rotationScheme != value)
                {
                    _rotationScheme = value;

                    OnChanged();
                }
            }
        }


        private ITransform _centerTransform;
        public ITransform CenterTransform
        {
            get => _centerTransform;
            set
            {
                if (_centerTransform != value)
                {
                    _centerTransform = value;

                    OnChanged();
                }
            }
        }

        private Vector3 _rotationAxis;
        public Vector3 RotationAxis
        {
            get => _rotationAxis;
            set
            {
                if (_rotationAxis != value)
                {
                    _rotationAxis = value;

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

        /// <param name="rotationAxis">Rotation axis</param>
        /// <param name="speed">Degrees per second</param>
        public TransformRotator(Vector3 rotationAxis, float speed)
        {
            RotationScheme = RotationType.AroundAxis;

            RotationAxis = rotationAxis;
            Speed = speed;
        }

        /// <param name="synchronousRotate">Rotete synchronous?</param>
        /// <param name="centerTransform">What transform it will be rotated around?</param>
        /// <param name="rotationAxis">Rotation axis</param>
        /// <param name="speed">Degrees per second</param>
        public TransformRotator(bool synchronousRotate, ITransform centerTransform, Vector3 rotationAxis, float speed)
        {
            if (synchronousRotate)
            {
                RotationScheme = RotationType.SynchronousAroundPoint;
            }
            else
            {
                RotationScheme = RotationType.AroundPoint;
            }

            CenterTransform = centerTransform;
            RotationAxis = rotationAxis;
            Speed = speed;
        }

        protected override void SubUpdate(uint frameTime)
        {
            float angularOffset = (float)frameTime / 1000 * (float)Math.PI / 180 * Speed;

            Rotate(ParentObject.Transform, angularOffset);
        }

        private void Rotate(ITransform transform, float angularOffset)
        {
            switch (RotationScheme)
            {
                case RotationType.AroundAxis:
                    transform.RotateAroundAxis(RotationAxis, angularOffset);
                    break;

                case RotationType.AroundPoint:
                    transform.RotateAroundPoint(CenterTransform.Position, RotationAxis, angularOffset);
                    break;

                case RotationType.SynchronousAroundPoint:
                    transform.SynchronousRotateAroundPoint(CenterTransform.Position, RotationAxis, angularOffset);
                    break;
            }
        }
    }
}
