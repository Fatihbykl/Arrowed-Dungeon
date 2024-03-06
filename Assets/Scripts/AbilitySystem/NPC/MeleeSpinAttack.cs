using Cysharp.Threading.Tasks;
using FSM;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/Melee Spin Attack")]
    public class MeleeSpinAttack : AbilityBase
    {
        public float focusTimeBeforeSpin;
        public float spinLength;
        
        private Enemy enemy;
        private TrailRenderer trail;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            enemy = owner.GetComponent<Enemy>();
            trail = owner.GetComponentInChildren<TrailRenderer>();
            
            enemy.castingAbility = true;
            enemy.animator.SetBool(AnimationParameters.CanSpin, true);
            StartSpin();
        }

        public override void BeginCooldown(GameObject owner, GameObject target)
        {
            //enemy.animator.SetBool(AnimationParameters.CanSpin, false);
        }
        
        private async void StartSpin()
        {
            // channeling
            enemy.agentController.agent.isStopped = true;
            
            await UniTask.WaitForSeconds(focusTimeBeforeSpin);

            enemy.agentController.agent.isStopped = false;
            trail.enabled = true;
            enemy.animator.SetTrigger(AnimationParameters.StartSpin);
            
            await UniTask.WaitForSeconds(spinLength);
            
            trail.enabled = false;
            enemy.animator.SetBool(AnimationParameters.CanSpin, false);
            enemy.castingAbility = false;
        }
    }
}
