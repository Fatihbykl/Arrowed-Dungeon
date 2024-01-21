using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Player;
using UnityEngine;
using UnityHFSM;

namespace FSM.Player.States
{
    public class AttackState : PlayerStateBase
    {
        private GameObject currentTarget;
        private float startTime;

        public AttackState(Gameplay.Player.Player player, StateMachine<PlayerState> playerFSM, bool needsExitTime)
            : base(player, playerFSM, needsExitTime: needsExitTime)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            startTime = Time.time;
            currentTarget = _player.currentTarget;

            _player.transform.DOLookAt(currentTarget.transform.position, 0.2f);
            _player.animator.SetTrigger(AnimationParameters.Attack);
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (currentTarget == null ||
                Time.time - startTime >= _player.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                _playerFSM.StateCanExit();
            }
        }

        public void OnSendArrow()
        {
            var direction = (currentTarget.transform.position - _player.transform.position).normalized;
            var force = direction * 25f;

            _player.arrow = GameObject.Instantiate(_player.arrowPrefab, _player.bow.transform.position,
                Quaternion.LookRotation(direction));
            _player.arrow.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}