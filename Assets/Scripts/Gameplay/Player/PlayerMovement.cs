using ECM.Controllers;
using FSM;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Gameplay.Player
{
    public class PlayerMovement : BaseCharacterController
    {
        public FloatingJoystick floatingJoystick;
        
        private Player _player;
        private PlayerStats _playerStats;
        
        void Start()
        {
            _player = GetComponent<Player>();
            _playerStats = GetComponent<PlayerStats>();
        }
        
        protected override void Animate()
        {
            base.Animate();

            if (moveDirection.magnitude >= 0.1f)
            {
                animator.SetBool(AnimationParameters.Moving, true);
                
                if (_player.attackModeActive)
                {
                    float value = 0.0f;
                    if (_player.currentTarget != null)
                    {
                        value = GetAngleBetween(moveDirection, transform.forward, transform.up);
                    }
                    _player.animator.SetFloat(AnimationParameters.Strafe, value);
                    speed = _playerStats.walkingSpeed.Value;
                }
                else
                {
                    speed = _playerStats.runningSpeed.Value;
                }
            }
            else
            {
                animator.SetBool(AnimationParameters.Moving, false);
            }
        }

        protected override void UpdateRotation()
        {
            if (_player.attackModeActive && _player.currentTarget != null)
            {
                RotateTowards((_player.currentTarget.transform.position - transform.position).normalized);
            }
            else
            {
                RotateTowardsMoveDirection();
            }
        }

        protected override void HandleInput()
        {
            base.HandleInput();
            
            var input = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
            moveDirection = new Vector3
            {
                x = input.x,
                y = 0.0f,
                z = input.z
            };
        }
        
        private float GetAngleBetween(Vector3 a, Vector3 b, Vector3 n){
            float signedAngle = Vector3.SignedAngle(a, b, n);
            float angle360 =  (signedAngle + 360) % 360;

            return angle360;
        }
    }
}
