using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Ranged Auto Attack")]
    public class RangedAutoAttack : AbilityBase
    {
        [Header("Projectile Settings")]
        public GameObject projectilePrefab;
        public GameObject projectileStartPosition;
        
        private Enemy enemy;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            enemy = owner.GetComponent<Enemy>();

            enemy.castingAbility = true;
            enemy.agentController.agent.isStopped = true;
            //enemy.agentController.agent.ResetPath();
            enemy.transform.DOLookAt(enemy.player.transform.position, .4f);
            enemy.animator.SetTrigger(AnimationParameters.Attack);
            
            SendProjectile(owner, target);
        }

        public override void BeginCooldown(GameObject owner, GameObject target)
        {
            enemy.agentController.agent.isStopped = false;
            enemy.castingAbility = false;
        }

        private void SendProjectile(GameObject owner, GameObject target)
        {
            var direction = (target.transform.position - owner.transform.position).normalized;
            var force = direction * 25f;

            var spawn = new Vector3(owner.transform.position.x, 1, owner.transform.position.z);
            var arrow = GameObject.Instantiate(projectilePrefab, spawn,
                Quaternion.LookRotation(direction));
            arrow.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}
