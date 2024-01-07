using System;
using UnityEngine;
using UnityHFSM;

namespace FSM.Player.States
{
    public class TakeDamageState : PlayerStateBase
    {
        public TakeDamageState(Gameplay.Player.Player player, StateMachine<PlayerState> playerFSM, Action<State<PlayerState, string>> onEnter = null, Action<State<PlayerState, string>> onLogic = null, Action<State<PlayerState, string>> onExit = null, Func<State<PlayerState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(player, playerFSM, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {}

        public override void OnEnter()
        {
            base.OnEnter();
            
            // foreach (var r in renderers)
            // {
            //     var currentColor = r.material.color;
            //     r.material.DOColor(animMaterial.color, .5f).From().SetEase(Ease.InFlash);
            //     r.material.DOColor(currentColor, .5f);
            // }
        }

        public void OnHit(int damage)
        {
            _player.animator.SetTrigger(AnimationParameters.TakeDamage);
            _player.playerHealth -= damage;
            _player.hpBar.UpdateHealthBar(_player.playerHealth);
        }
    }
}