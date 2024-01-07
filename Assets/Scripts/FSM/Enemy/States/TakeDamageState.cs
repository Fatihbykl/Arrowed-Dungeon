using System;
using DG.Tweening;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public class TakeDamageState : EnemyStateBase
    {
        public TakeDamageState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        { }

        public void OnHit(int damage)
        {
            StartTakeDamageAnim();
            _enemy.currentHealth -= damage;
            _enemy.hpBar.UpdateHealthBar(_enemy.currentHealth);
        }
        
        private void StartTakeDamageAnim()
        {
            _enemy.animator.SetTrigger(AnimationParameters.TakeDamage);
            _enemy.meshRenderer.material.DOColor(Color.red, .5f).From().SetEase(Ease.InFlash);
        }
    }
}