using System;
using FSM.Enemy;
using FSM.Enemy.States;
using FSM.Enemy.States.FootlessSkeletonStates.Archer;
using FSM.Enemy.States.FootlessSkeletonStates.Minion;
using Gameplay.DamageDealers;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using NaughtyAttributes;
using UnityEngine;
using UnityHFSM;

namespace Gameplay.Enemy.FootlessSkeleton
{
    public class FootlessMinion : Enemy, IDealDamage
    {
        [Header("Spin Settings")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]

        [Foldout("FL Minion Settings")] public float focusTimeBeforeSpin;
        [Foldout("FL Minion Settings")]public float spinLength;
        [Foldout("FL Minion Settings")] [Range(0, 1)] public float triggerHealthPercentage;
        [Foldout("FL Minion Settings")] public float spinCooldown;
        
        private WeaponDamageDealer damageDealer;
        [HideInInspector] public float lastSpinTime = 0;
        [HideInInspector] public bool canSpin = false;
        
        private void OnEnable()
        {
            damageDealer = GetComponentInChildren<WeaponDamageDealer>();
            
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(this, EnemyFSM, needsExitTime:true));
            EnemyFSM.AddState(EnemyState.Spin, new SpinState(this, EnemyFSM));
            
            EnemyFSM.AddTriggerTransition("OnAttack", new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack));
            EnemyFSM.AddTransition(EnemyState.Attack, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Chase, EnemyState.Spin, t => canSpin, forceInstantly:true);
            EnemyFSM.AddTransition(EnemyState.Spin, EnemyState.Chase, t => !canSpin);
            
            EnemyFSM.SetStartState(EnemyState.Patrol);
            EnemyFSM.Init();
        }

        private void LateUpdate()
        {
            var healthPercentage = currentHealth / (float)enemySettings.enemyBaseHealth;

            if (healthPercentage <= triggerHealthPercentage && Time.time - lastSpinTime >= spinCooldown)
            {
                canSpin = true;
            }
        }

        public void StartDealDamage()
        {
            damageDealer.OnStartDealDamage(this.gameObject);
        }

        public void EndDealDamage()
        {
            damageDealer.OnEndDealDamage(this.gameObject);
        }
    }
}