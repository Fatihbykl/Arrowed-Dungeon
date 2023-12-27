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
        }

        public override void OnLogic()
        {
            base.OnLogic();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}