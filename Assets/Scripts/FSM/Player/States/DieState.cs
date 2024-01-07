using System;
using UnityEngine;
using UnityHFSM;

namespace FSM.Player.States
{
    public class DieState : PlayerStateBase
    {
        public DieState(Gameplay.Player.Player player, StateMachine<PlayerState> playerFSM, Action<State<PlayerState, string>> onEnter = null, Action<State<PlayerState, string>> onLogic = null, Action<State<PlayerState, string>> onExit = null, Func<State<PlayerState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(player, playerFSM, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Die();
        }
        
        private void Die()
        {
            _player.playerCollider.enabled = false;
            _player.characterController.enabled = false;
            _player.moveAction.Disable();
            
            _player.animator.SetTrigger(AnimationParameters.Die);
        }
    }
}