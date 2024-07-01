using Animations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Melee Spin Attack")]
    public class MeleeSpinAttack : AbilityBase
    {
        public float focusTimeBeforeSpin;
        public float spinLength;
        
        private Enemy.Enemy _enemy;

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy.Enemy>();
        }

        public override void Activate(GameObject target)
        {
            _enemy.castingAbility = true;
            _enemy.animator.SetBool(AnimationParameters.CanSpin, true);
            StartSpin();
        }

        public override void BeginCooldown()
        {
            _enemy.castingAbility = false;
            _enemy.animator.SetBool(AnimationParameters.CanSpin, false);
        }
        
        private async void StartSpin()
        {
            _enemy.agentController.speed = 0;
            await UniTask.WaitForSeconds(focusTimeBeforeSpin);
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
            await UniTask.WaitForSeconds(spinLength);
        }
    }
}
