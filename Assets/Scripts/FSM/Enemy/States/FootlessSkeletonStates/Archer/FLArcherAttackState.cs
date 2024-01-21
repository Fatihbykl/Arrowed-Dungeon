using System;
using DG.Tweening;
using Gameplay.Enemy.FootlessSkeleton;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States.FootlessSkeletonStates.Archer
{
    public class FLArcherAttackState : EnemyStateBase
    {
        private float animStartTime;
        private FootlessArcher archer;
        
        public FLArcherAttackState(FootlessArcher enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
            archer = enemy;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            archer.agent.isStopped = true;
            archer.agent.ResetPath();
            archer.transform.DOLookAt(archer.player.transform.position, .4f);
            archer.animator.SetTrigger(AnimationParameters.Attack);
            animStartTime = Time.time;
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (Time.time - animStartTime > archer.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                archer.agent.isStopped = false;
                _enemyFSM.StateCanExit();
            }
        }

        public void OnSendArrow()
        {
            var direction = (archer.player.transform.position - archer.transform.position).normalized;
            var force = direction * 25f;

            var arrow = GameObject.Instantiate(archer.arrowPrefab, archer.arrowStartPosition.transform.position,
                Quaternion.LookRotation(direction));
            arrow.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}
