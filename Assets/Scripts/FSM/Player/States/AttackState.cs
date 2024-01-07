using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityHFSM;

namespace FSM.Player.States
{
    public class AttackState : PlayerStateBase, IActionable<string>
    {
        private int numClicks;
        private Collider currentTarget;

        public AttackState(Gameplay.Player.Player player, StateMachine<PlayerState> playerFSM, bool needsExitTime)
            : base(player, playerFSM, needsExitTime: needsExitTime)
        {}

        public override void OnEnter()
        {
            base.OnEnter();

            numClicks = 0;
            currentTarget = _player.currentTarget;

            _player.transform.DOLookAt(currentTarget.transform.position, .5f);
            _player.animator.SetTrigger(AnimationParameters.Attack);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            if (currentTarget == null) { _playerFSM.StateCanExit(); }

            if (_player.attackAction.triggered)
            {
                numClicks++;
            }
        }
        
        public void OnSendArrow()
        {
            var direction = (currentTarget.transform.position - _player.transform.position).normalized;
            var force = direction * 25f;
            
            _player.arrow = GameObject.Instantiate(_player.arrowPrefab, _player.bow.transform.position, Quaternion.LookRotation(direction));
            _player.arrow.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }

        public void OnCheckAttackClicks()
        {
            if (numClicks > 1)
            {
                _player.animator.CrossFade("Arrow Attack", 0.25f, 0);
                numClicks = 0;
            }
            else
            {
                numClicks = 0;
                _playerFSM.StateCanExit();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}