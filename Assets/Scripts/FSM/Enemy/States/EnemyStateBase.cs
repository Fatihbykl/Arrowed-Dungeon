using System;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public abstract class EnemyStateBase : State<EnemyState>
    {
        protected readonly Gameplay.Enemy.Enemy _enemy;
        protected readonly StateMachine<EnemyState> _enemyFSM;


        protected EnemyStateBase(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
            _enemy = enemy;
            _enemyFSM = enemyFsm;
        }
    }
}
