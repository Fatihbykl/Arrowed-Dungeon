﻿using UnityEngine;

namespace Gameplay.Movement.Helpers
{
    /// <summary>
    ///
    /// RootMotionController.
    /// 
    /// Helper component to get 'Animator' root-motion velocity vector (animVelocity) and angular velocity vector (animAngularVelocity).
    /// This must be attached to a game object with an 'Animator' component.
    /// 
    /// </summary>

    [RequireComponent(typeof(Animator))]
    public sealed class RootMotionController : MonoBehaviour
    {
        #region FIELDS

        private Animator _animator;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// The animation velocity vector.
        /// </summary>

        public Vector3 animVelocity { get; private set; }

        /// <summary>
        /// The animation angular velocity vector.
        /// </summary>

        public Vector3 animAngularVelocity { get; private set; }

        /// <summary>
        /// The animation delta rotation from the last evaluated frame.
        /// </summary>

        public Quaternion animDeltaRotation { get; private set; }

        #endregion

        #region MONOBEHAVIOUR

        public void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_animator == null)
            {
                Debug.LogError(
                    string.Format(
                        "RootMotionController: There is no 'Animator' attached to the '{0}' game object.\n" +
                        "Please attach a 'Animator' to the '{0}' game object",
                        name));
            }
        }

        public void OnAnimatorMove()
        {
            // Compute velocities from animation

            var deltaTime = Time.deltaTime;
            if (deltaTime <= 0.0f)
                return;

            // Compute animation velocity

            animVelocity = _animator.deltaPosition / deltaTime;

            // Compute animation angular velocity

            animDeltaRotation = _animator.deltaRotation;

            float angleInDegrees;
            Vector3 rotationAxis;
            animDeltaRotation.ToAngleAxis(out angleInDegrees, out rotationAxis);

            Vector3 angularDisplacement = rotationAxis * angleInDegrees * Mathf.Deg2Rad;
            animAngularVelocity = angularDisplacement / Time.deltaTime;
        }

        #endregion
    }
}