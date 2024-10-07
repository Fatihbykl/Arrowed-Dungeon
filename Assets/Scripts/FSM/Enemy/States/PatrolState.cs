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
            
            _enemy.waypointReached = false;
            _enemy.agentController.agent.isStopped = false;
            _enemy.agentController.speed = _enemy.patrolSpeed;
            _enemy.agentController.agent.SetDestination(_enemy.waypointObject.transform.GetChild(_enemy.currentWaypoint).position);
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (_enemy.agentController.agent.pathPending) { return; }
            if (!_enemy.agentController.agent.hasPath)
            {
                _enemy.waypointReached = true;
            }
        }

        private void NextWaypoint()
        {
            _enemy.currentWaypoint = (_enemy.currentWaypoint + 1) % _enemy.waypointObject.transform.childCount;
        }

        public override void OnExit()
        {
            base.OnExit();
            
            NextWaypoint();
        }
    }
}