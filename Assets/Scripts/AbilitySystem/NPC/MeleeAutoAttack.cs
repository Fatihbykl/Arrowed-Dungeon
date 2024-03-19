using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Melee Auto Attack")]
    public class MeleeAutoAttack : AbilityBase
    {
        private Enemy enemy;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            enemy = owner.GetComponent<Enemy>();

            enemy.castingAbility = true;
            enemy.agentController.agent.isStopped = true;
            enemy.agentController.agent.ResetPath();
            enemy.transform.DOLookAt(enemy.player.transform.position, .4f);
            enemy.animator.SetTrigger(AnimationParameters.Attack);
        }

        public override void BeginCooldown(GameObject owner, GameObject target)
        {
            enemy.agentController.agent.isStopped = false;
            enemy.castingAbility = false;
        }
    }
}
