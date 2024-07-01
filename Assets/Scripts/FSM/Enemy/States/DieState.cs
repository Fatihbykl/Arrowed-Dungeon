using System;
using Animations;
using Cysharp.Threading.Tasks;
using Gameplay.Loot;
using Gameplay.Managers;
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

            AIManager.Instance.units.Remove(_enemy);
            _enemy.agentController.speed = 0;
            _enemy.agentController.agent.ResetPath();
            _enemy.GetComponent<LootSpawner>().SpawnItems();
            Die();
        }

        private async void Die()
        {
            // deactivate hp bar and collider for prevent further attacks
            _enemy.capsuleCollider.enabled = false;
            _enemy.hpBar.gameObject.SetActive(false);
            _enemy.GetComponentInChildren<ParticleSystem>().Play();
            
            // play animation
            _enemy.animator.SetTrigger(AnimationParameters.Die);
            await UniTask.WaitForSeconds(2.5f);
            
            // fade out animation
            // await _enemy.meshRenderer.material.DOFade(0f, 1f).ToUniTask();
            
            GameObject.Destroy(_enemy.gameObject.transform.parent.gameObject);
        }
    }
}