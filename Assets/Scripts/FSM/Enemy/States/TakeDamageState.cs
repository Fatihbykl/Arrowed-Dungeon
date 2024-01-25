using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public class TakeDamageState : EnemyStateBase
    {
        private float blinkTimer;
        
        public TakeDamageState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        { }

        public void OnHit(int damage)
        {
            StartTakeDamageAnim();
            _enemy.currentHealth -= damage;
            _enemy.hpBar.UpdateHealthBar(_enemy.currentHealth);
            _enemy.playerDetected = true;

            blinkTimer = _enemy.blinkDuration;
        }

        private async void StartTakeDamageAnim()
        {
            await _enemy.meshRenderer.material.DOColor(Color.white * _enemy.blinkIntensity, _enemy.blinkDuration / 2).ToUniTask();
            await _enemy.meshRenderer.material.DOColor(Color.white, _enemy.blinkDuration / 2).ToUniTask();
        }
    }
}