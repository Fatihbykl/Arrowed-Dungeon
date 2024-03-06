using System;
using System.Linq;
using AbilitySystem;
using DG.Tweening;
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
            
            _enemy.agentController.agent.isStopped = false;
            _enemy.agentController.speed = _enemy.enemySettings.chaseSpeed;
            _enemy.agentController.agent.stoppingDistance = _enemy.enemySettings.stoppingDistance;
        }

        public override void OnLogic()
        {
            base.OnLogic();

            _enemy.agentController.agent.SetDestination(_enemy.player.transform.position);
            
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
                .FirstOrDefault(abilityHolder => abilityHolder.ability.currentState == AbilityBase.AbilityState.Ready &&
                                                 abilityHolder.ability.castRange >= distanceToTarget);
            return ability;
        }
    }
}