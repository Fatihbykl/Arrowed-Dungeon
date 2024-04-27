using Cysharp.Threading.Tasks;
using FSM;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Melee Spin Attack")]
    public class MeleeSpinAttack : AbilityBase
    {
        public float focusTimeBeforeSpin;
        public float spinLength;
        
        private Enemy enemy;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            enemy = owner.GetComponent<Enemy>();
            
            enemy.castingAbility = true;
            enemy.animator.SetBool(AnimationParameters.CanSpin, true);
            StartSpin();
        }

        public override void BeginCooldown()
        {
            enemy.castingAbility = false;
            enemy.animator.SetBool(AnimationParameters.CanSpin, false);
        }
        
        private async void StartSpin()
        {
            enemy.agentController.speed = 0;
            await UniTask.WaitForSeconds(focusTimeBeforeSpin);
            enemy.agentController.speed = enemy.enemySettings.chaseSpeed;
            await UniTask.WaitForSeconds(spinLength);
        }
    }
}
