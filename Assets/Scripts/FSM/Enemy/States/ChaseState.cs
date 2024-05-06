using System;
using System.Linq;
using AbilitySystem;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States
{
    public class ChaseState : EnemyStateBase
    {
        public ChaseState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm,
            Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null,
            Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null,
            bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit,
            canExit, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            AIManager.Instance.Units.Add(_enemy);
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
            _enemy.agentController.stoppingDistance = _enemy.stoppingDistance;
        }

        public override void OnLogic()
        {
            base.OnLogic();

            //if (!_enemy.castingAbility) { _enemy.transform.DOLookAt(_enemy.player.transform.position, 0.5f); }
            var ability = GetAbility();
            if (ability)
            {
                ability.ActivateAbility();
            }
        }

        private AbilityHolder GetAbility()
        {
            if (_enemy.castingAbility) { return null; }
            
            var distanceToTarget = Vector3.Distance(_enemy.transform.position, _enemy.player.transform.position);
            var ability = _enemy.abilityHolders
                .FirstOrDefault(abilityHolder => abilityHolder.currentState == AbilityHolder.AbilityState.Ready &&
                                                 abilityHolder.ability.castRange >= distanceToTarget);
            
            return ability;
        }
    }
}