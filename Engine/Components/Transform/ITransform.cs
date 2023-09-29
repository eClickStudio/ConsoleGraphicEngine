using Engine3D.Components.Abstract;
using Hierarchy;
using System;
using System.Numerics;

namespace Engine3D.Components.Transform
{
    public interface ITransform : IComponent, IHierarchyMember<ITransform>
    {
        /// <summary>
        /// Position in world space
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Position in local space
        /// </summary>
        Vector3 LocalPosition { get; set; }

        void UpdatePosition();

        /// <summary>
        /// Triggers on position change
        /// </summary>
        event Action onPositionChangedEvent;

        #region Rotation
        /// <summary>
        /// Axis X relative to this object
        /// </summary>
        Vector3 AxisX { get; }

        /// <summary>
        /// Axis Y relative to this object
        /// </summary>
        Vector3 AxisY { get; }

        /// <summary>
        /// Axis Z relative to this object
        /// </summary>
        Vector3 AxisZ { get; }

        /// <summary>
        /// Triggers on rotation change
        /// </summary>
        event Action OnRotationChangedEvent;

        /// <summary>
        /// Rotates around its axis
        /// </summary>
        /// <param name="rotationAxis">Vector of axis</param>
        /// <param name="angle">Angle in radians</param>
        void RotateAroundAxis(Vector3 rotationAxis, float angle);

        /// <summary>
        /// Rotates around world position (Keeps the same distance to this point)
        /// </summary>
        /// <param name="point">World position of point you want to rotate around</param>
        /// <param name="rotationAxis">Vector of axis</param>
        /// <param name="angle">Angle in radians</param>
        void RotateAroundPoint(Vector3 point, Vector3 rotationAxis, float angle);

        /// <summary>
        /// Rotates around world position (Keeps the same distance to this point) 
        /// and it remains directed to the point by the same side
        /// </summary>
        /// <param name="point">World position of point you want to rotate around</param>
        /// <param name="rotationAxis">Vector of axis</param>
        /// <param name="angle">Angle in radians</param>
        void SynchronousRotateAroundPoint(Vector3 point, Vector3 rotationAxis, float angle);
        #endregion
    }
}
