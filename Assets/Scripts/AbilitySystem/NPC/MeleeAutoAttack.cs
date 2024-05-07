using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Melee Auto Attack")]
    public class MeleeAutoAttack : AbilityBase
    {
        private Enemy _enemy;

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy>();
        }

        public override void Activate(GameObject target)
        {
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0;
            _enemy.agentController.agent.ResetPath();
            _enemy.transform.DOLookAt(_enemy.player.transform.position, .4f);
            _enemy.animator.SetTrigger(AnimationParameters.Attack);
        }

        public override void BeginCooldown()
        {
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
            _enemy.castingAbility = false;
        }
    }
}
