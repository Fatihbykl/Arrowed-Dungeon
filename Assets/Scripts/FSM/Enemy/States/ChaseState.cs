using System;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public class ChaseState : EnemyStateBase
    {
        private float lastAttackTime;
        
        public ChaseState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            _enemy.animator.SetBool(AnimationParameters.Chase, true);
            _enemy.agent.isStopped = false;
            _enemy.agent.speed = _enemy.chaseSpeed;
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            _enemy.agent.SetDestination(_enemy.player.transform.position);
            if (_enemy.agent.remainingDistance <= _enemy.agent.stoppingDistance)
            {
                lastAttackTime = Time.time;
                _enemyFSM.Trigger("OnAttack");
            }
        }
    }
}