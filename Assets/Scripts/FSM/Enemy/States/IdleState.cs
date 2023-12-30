using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public class IdleState : EnemyStateBase
    {
        public IdleState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _enemy.agent.isStopped = true;
            _enemy.agent.speed = 0f;
            
            WaitBeforeMoveNextWaypoint();
        }

        private async void WaitBeforeMoveNextWaypoint()
        {
            _enemy.canMoveNextWaypoint = false;
            await UniTask.WaitForSeconds(_enemy.enemySettings.waypointWaitTime);
            _enemy.canMoveNextWaypoint = true;
        }
    }
}