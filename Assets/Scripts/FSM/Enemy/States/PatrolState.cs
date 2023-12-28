using System;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public class PatrolState : EnemyStateBase
    {
        public PatrolState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _enemy.animator.SetBool(AnimationParameters.Chase, false);
            _enemy.animator.SetBool(AnimationParameters.Patrol, true);
            _enemy.waypointReached = false;
            _enemy.agent.isStopped = false;
            _enemy.agent.speed = _enemy.patrolSpeed;
            _enemy.agent.SetDestination(_enemy.waypoints[_enemy.currentWaypoint].position);
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (_enemy.agent.pathPending) { return; }
            if (_enemy.agent.remainingDistance <= _enemy.agent.stoppingDistance)
            {
                NextWaypoint();
                _enemy.waypointReached = true;
            }
        }

        private void NextWaypoint()
        {
            _enemy.currentWaypoint = (_enemy.currentWaypoint + 1) % _enemy.waypoints.Length;
        }

        public override void OnExit()
        {
            base.OnExit();
            
            _enemy.animator.SetBool(AnimationParameters.Patrol, false);
        }
    }
}