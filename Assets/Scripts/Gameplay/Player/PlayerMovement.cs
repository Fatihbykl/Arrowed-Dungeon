using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector3 velocity;
        private Vector3 direction;
        private float speed = 1;
        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;
        private InputAction moveAction;
        private CharacterController characterController;
        private Player player;
    
        void Start()
        {
            moveAction = GetComponent<PlayerInput>().actions["Move"];
            characterController = GetComponent<CharacterController>();
            player = GetComponent<Player>();
        }

    
        void Update()
        {
            var input = moveAction.ReadValue<Vector2>();
            float horizontal = input.x;
            float vertical = input.y;
            
            direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= 0.1f && player.canMove)
            {
                
                player.animator.SetBool(AnimationParameters.Moving, true);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    targetAngle,
                    ref turnSmoothVelocity,
                    turnSmoothTime
                );
                
                if (player.attackModeActive)
                {
                    var value = GetAngleBetween(direction, transform.forward, transform.up);
                    player.animator.SetFloat(AnimationParameters.Strafe, value);
                    speed = player.stats.walkingSpeed;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    speed = player.stats.runningSpeed;
                }
                characterController.Move(direction.normalized * speed * Time.deltaTime);
            }
            else
            {
                player.animator.SetBool(AnimationParameters.Moving, false);
            }
        }
        
        private float GetAngleBetween(Vector3 a, Vector3 b, Vector3 n){
            float signedAngle = Vector3.SignedAngle(a, b, n);
            float angle360 =  (signedAngle + 360) % 360;

            return angle360;
        }
    }
}
