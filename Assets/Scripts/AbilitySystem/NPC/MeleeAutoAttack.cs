using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Melee Auto Attack")]
    public class MeleeAutoAttack : AbilityBase
    {
        private Enemy enemy;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            enemy = owner.GetComponent<Enemy>();

            enemy.castingAbility = true;
            enemy.agentController.speed = 0;
            enemy.agentController.agent.ResetPath();
            enemy.transform.DOLookAt(enemy.player.transform.position, .4f);
            enemy.animator.SetTrigger(AnimationParameters.Attack);
        }

        public override void BeginCooldown()
        {
            enemy.agentController.speed = enemy.enemyStats.chaseSpeed.Value;
            enemy.castingAbility = false;
        }
    }
}
