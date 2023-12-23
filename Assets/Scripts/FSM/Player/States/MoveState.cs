using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityHFSM;

namespace FSM.Player.States
{
    public class MoveState : PlayerStateBase
    {
        public CharacterController controller;
        public float speed = 3;
        public float gravity = -9.81f;
        public Vector3 direction;
        public float turnSmoothTime = 0.1f;
        public bool isActive = true;
        private Vector3 velocity;
        private float turnSmoothVelocity;

        public MoveState(Gameplay.Player.Player player, StateMachine<PlayerState> _playerFSM) : base(player, _playerFSM) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _player.animator.SetBool(AnimationParameters.Moving, true);
            speed = GameManager.instance.playerSpeed;
            controller = _player.GetComponent<CharacterController>();
        }

        public override void OnLogic()
        {
            base.OnLogic();

            var input = _player.moveAction.ReadValue<Vector2>();
            float horizontal = input.x;
            float vertical = input.y;
            
            direction = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(
                    _player.transform.eulerAngles.y,
                    targetAngle,
                    ref turnSmoothVelocity,
                    turnSmoothTime
                );
                _player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                controller.Move(direction.normalized * speed * Time.deltaTime);
            }
            else
            {
                //_playerFSM.RequestStateChange(PlayerState.Idle);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _player.animator.SetBool(AnimationParameters.Moving, false);
        }
    }
}