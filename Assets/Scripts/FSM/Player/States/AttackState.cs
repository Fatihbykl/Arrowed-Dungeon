using System;
using UnityEngine;
using UnityHFSM;

namespace FSM.Player.States
{
    public class AttackState : PlayerStateBase
    {
        private float lastClickTime;
        private int numClicks;
        private float clipLength;
        private float clipSpeed;

        public AttackState(Gameplay.Player.Player player, StateMachine<PlayerState> playerFSM, bool needsExitTime)
            : base(player, playerFSM, needsExitTime: needsExitTime)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            numClicks = 0;
            lastClickTime = 0;

            _player.animator.applyRootMotion = true;
            _player.animator.SetBool(AnimationParameters.AttackStart, true);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            if (Time.time - lastClickTime > _player.comboDelay)
            {
                numClicks = 0;
                _playerFSM.StateCanExit();
                //Debug.Log("Exit State");
            }
            
            if (_player.attackAction.triggered)
            {
                lastClickTime = Time.time;
                numClicks++;
            
                if (numClicks <= AnimationParameters.AttackCombos.Length)
                {
                    _player.animator.SetTrigger(AnimationParameters.AttackCombos[numClicks - 1]);
                    Debug.Log($"Attacked - {numClicks}");
                }
                else
                {
                    _playerFSM.StateCanExit();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _player.animator.applyRootMotion = false;
            _player.animator.SetBool(AnimationParameters.AttackStart, false);
        }
    }
}