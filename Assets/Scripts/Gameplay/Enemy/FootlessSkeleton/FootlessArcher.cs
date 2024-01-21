using System;
using FSM.Enemy;
using FSM.Enemy.States.FootlessSkeletonStates.Archer;
using UnityEngine;
using UnityHFSM;

namespace Gameplay.Enemy.FootlessSkeleton
{
    public class FootlessArcher : Enemy
    {
        [Header("Settings For Archer Enemies")]
        public GameObject arrowPrefab;
        public GameObject arrowStartPosition;

        private void OnEnable()
        {
            var attackState = new FLArcherAttackState(this, EnemyFSM, needsExitTime:true);
            
            EnemyFSM.AddState(EnemyState.Attack,
                attackState.AddAction("OnSendArrow", () => attackState.OnSendArrow()));
            
            EnemyFSM.AddTriggerTransition("OnAttack", new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack));
            EnemyFSM.AddTransition(EnemyState.Attack, EnemyState.Chase, t => playerDetected);
            
            EnemyFSM.SetStartState(EnemyState.Patrol);
            EnemyFSM.Init();
        }
        
        public void SendArrow()
        {
            EnemyFSM.OnAction("OnSendArrow");
        }
    }
}
