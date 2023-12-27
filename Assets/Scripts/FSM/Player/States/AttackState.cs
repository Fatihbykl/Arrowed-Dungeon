using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityHFSM;

namespace FSM.Player.States
{
    public class AttackState : PlayerStateBase
    {
        private float lastClickTime;
        private int numClicks;
        private bool isComboEnded;
        private float clipLength;
        private float clipSpeed;

        public AttackState(Gameplay.Player.Player player, StateMachine<PlayerState> playerFSM, bool needsExitTime)
            : base(player, playerFSM, needsExitTime: needsExitTime)
        {
            GameplayEvents.AttackTransition += OnComboAttackTransition;
        }
        
        

        public override void OnEnter()
        {
            base.OnEnter();

            numClicks = 0;
            lastClickTime = 0;
            isComboEnded = false;

            //_player.animator.applyRootMotion = true;
            _player.animator.SetBool(AnimationParameters.AttackStart, true);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            if (Time.time - lastClickTime > _player.animator.GetCurrentAnimatorStateInfo(0).length && _player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f)
            {
                numClicks = 0;
                _playerFSM.StateCanExit();
            }
            
            if (_player.attackAction.triggered && !isComboEnded)
            {
                lastClickTime = Time.time;
                numClicks++;
            
                if (numClicks == 1)
                {
                    _player.animator.SetTrigger(AnimationParameters.AttackCombos[0]);
                }

                numClicks = Mathf.Clamp(numClicks, 0, 3);
            }
        }

        private void OnComboAttackTransition(int transitionNumber)
        {
            if (numClicks >= 2 && transitionNumber == 1)
            {
                _player.animator.SetTrigger(AnimationParameters.AttackCombos[1]);
                lastClickTime = Time.time;
            }
            if (numClicks >= 3 && transitionNumber == 2)
            {
                if (isComboEnded) { return; }
                _player.animator.SetTrigger(AnimationParameters.AttackCombos[2]);
                lastClickTime = Time.time;
                isComboEnded = true;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            //_player.animator.applyRootMotion = false;
            _player.animator.SetBool(AnimationParameters.AttackStart, false);
        }
    }
}