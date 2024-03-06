using ECM.Controllers;
using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerMovement : BaseCharacterController
    {
        private InputAction moveAction;
        private Player player;
        
        void Start()
        {
            moveAction = GetComponent<PlayerInput>().actions["Move"];
            player = GetComponent<Player>();
        }
        
        protected override void Animate()
        {
            base.Animate();

            if (moveDirection.magnitude >= 0.1f)
            {
                animator.SetBool(AnimationParameters.Moving, true);
                
                if (player.attackModeActive)
                {
                    float value = 0.0f;
                    if (player.currentTarget != null)
                    {
                        value = GetAngleBetween(moveDirection, transform.forward, transform.up);
                    }
                    player.animator.SetFloat(AnimationParameters.Strafe, value);
                    speed = player.stats.walkingSpeed;
                }
                else
                {
                    speed = player.stats.runningSpeed;
                }
            }
            else
            {
                animator.SetBool(AnimationParameters.Moving, false);
            }
        }

        protected override void UpdateRotation()
        {
            if (player.attackModeActive && player.currentTarget != null)
            {
                RotateTowards((player.currentTarget.transform.position - transform.position).normalized);
            }
            else
            {
                RotateTowardsMoveDirection();
            }
        }

        protected override void HandleInput()
        {
            base.HandleInput();
            
            var input = moveAction.ReadValue<Vector2>();
            moveDirection = new Vector3
            {
                x = input.x,
                y = 0.0f,
                z = input.y
            };
        }
        
        private float GetAngleBetween(Vector3 a, Vector3 b, Vector3 n){
            float signedAngle = Vector3.SignedAngle(a, b, n);
            float angle360 =  (signedAngle + 360) % 360;

            return angle360;
        }
    }
}
