using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public class DieState : EnemyStateBase
    {
        public DieState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            
            AIManager.Instance.RemoveUnit(_enemy);
            Die();
        }

        private async void Die()
        {
            // deactivate hp bar and collider for prevent further attacks
            _enemy.capsuleCollider.enabled = false;
            _enemy.hpBar.gameObject.SetActive(false);
            _enemy.GetComponentInChildren<ParticleSystem>().Play();
            AudioManager.instance.PlayArrowCoinsDropSFX();
            
            // play animation
            _enemy.animator.SetTrigger(AnimationParameters.Die);
            var dieAnimation = _enemy.animator.GetCurrentAnimatorStateInfo(0);
            await UniTask.WaitForSeconds(dieAnimation.length / dieAnimation.speed);
            
            // fade out animation
            // await _enemy.meshRenderer.material.DOFade(0f, 1f).ToUniTask();
            
            GameObject.Destroy(_enemy.gameObject, 2f);
        }
    }
}