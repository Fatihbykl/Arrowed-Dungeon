using System;
using DG.Tweening;
using UnityEngine;
using UnityHFSM;
using Vector3 = UnityEngine.Vector3;

namespace FSM.Enemy.States
{
    public class AttackState : EnemyStateBase
    {
        private float animStartTime;
        
        public AttackState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _enemy.agent.isStopped = true;
            _enemy.agent.ResetPath();
            _enemy.transform.DOLookAt(_enemy.player.transform.position, .4f);
            _enemy.animator.SetTrigger(AnimationParameters.Attack);
            animStartTime = Time.time;
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (Time.time - animStartTime > _enemy.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                _enemy.agent.isStopped = false;
                _enemyFSM.StateCanExit();
            }

        }
    }
}