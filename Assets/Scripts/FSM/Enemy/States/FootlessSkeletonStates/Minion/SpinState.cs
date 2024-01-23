using System;
using Cysharp.Threading.Tasks;
using Gameplay.Enemy.FootlessSkeleton;
using UnityEngine;
using UnityHFSM;

namespace FSM.Enemy.States.FootlessSkeletonStates.Minion
{
    public class SpinState : EnemyStateBase
    {
        private FootlessMinion enemy;
        private bool canMove = false;
        
        public SpinState(Gameplay.Enemy.Enemy enemy, StateMachine<EnemyState> enemyFsm, Action<State<EnemyState, string>> onEnter = null, Action<State<EnemyState, string>> onLogic = null, Action<State<EnemyState, string>> onExit = null, Func<State<EnemyState, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false) : base(enemy, enemyFsm, onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            enemy = _enemy.GetComponent<FootlessMinion>();
            enemy.animator.SetBool(AnimationParameters.CanSpin, true);
            StartSpin();
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (canMove)
            {
                enemy.agent.isStopped = false;
                enemy.agent.SetDestination(_enemy.player.transform.position);
            }
            else
            {
                enemy.agent.isStopped = true;
            }
        }

        private async void StartSpin()
        {
            // channeling
            Debug.Log("Channeling");
            await UniTask.WaitForSeconds(enemy.focusTimeBeforeSpin);
            
            Debug.Log("Spinning");
            canMove = true;
            enemy.animator.SetTrigger(AnimationParameters.StartSpin);
            await UniTask.WaitForSeconds(enemy.spinLength);
            enemy.canSpin = false;
            canMove = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            
            enemy.animator.SetBool(AnimationParameters.CanSpin, false);
            enemy.lastSpinTime = Time.time;
        }
    }
}