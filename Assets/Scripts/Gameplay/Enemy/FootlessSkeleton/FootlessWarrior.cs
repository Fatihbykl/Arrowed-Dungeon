using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.DamageDealers;
using UnityHFSM;

namespace Gameplay.Enemy.FootlessSkeleton
{
    public class FootlessWarrior : Enemy
    {
        private WeaponDamageDealer damageDealer;
        
        private void OnEnable()
        {
            damageDealer = GetComponentInChildren<WeaponDamageDealer>();
            
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(this, EnemyFSM));
            
            EnemyFSM.AddTriggerTransition("OnAttack", new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack));
            EnemyFSM.AddTransition(EnemyState.Attack, EnemyState.Chase, t => playerDetected);
            
            EnemyFSM.SetStartState(EnemyState.Patrol);
            EnemyFSM.Init();
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