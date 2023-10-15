using System.Numerics;
using Engine3D.Components.Transform;
using Engine3D.Components.Abstract;

//Here you can add your custom scripts
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

        private RotationType _rotationType { get; }
        private ITransform _centerTransform { get; }
        private Vector3 _rotationAxis { get; }
        private float _speed { get; }

        /// <param name="rotationAxis">Rotation axis</param>
        /// <param name="speed">Degrees per second</param>
        public TransformRotator(Vector3 rotationAxis, float speed)
        {
            _rotationType = RotationType.AroundAxis;

            _rotationAxis = rotationAxis;
            _speed = speed;
        }

        /// <param name="synchronousRotate">Rotete synchronous?</param>
        /// <param name="centerTransform">What transform it will be rotated around?</param>
        /// <param name="rotationAxis">Rotation axis</param>
        /// <param name="speed">Degrees per second</param>
        public TransformRotator(bool synchronousRotate, ITransform centerTransform, Vector3 rotationAxis, float speed)
        {
            if (synchronousRotate)
            {
                _rotationType = RotationType.SynchronousAroundPoint;
            }
            else
            {
                _rotationType = RotationType.AroundPoint;
            }

            _centerTransform = centerTransform;
            _rotationAxis = rotationAxis;
            _speed = speed;
        }

        protected override void SubUpdate(uint frameTime)
        {
            float angularOffset = (float)frameTime / 1000 * (float)Math.PI / 180 * _speed;

            Rotate(ParentObject.Transform, angularOffset);
        }

        private void Rotate(ITransform transform, float angularOffset)
        {
            switch (_rotationType)
            {
                case RotationType.AroundAxis:
                    transform.RotateAroundAxis(_rotationAxis, angularOffset);
                    break;

                case RotationType.AroundPoint:
                    transform.RotateAroundPoint(_centerTransform.Position, _rotationAxis, angularOffset);
                    break;

                case RotationType.SynchronousAroundPoint:
                    transform.SynchronousRotateAroundPoint(_centerTransform.Position, _rotationAxis, angularOffset);
                    break;
            }
        }
    }
}
