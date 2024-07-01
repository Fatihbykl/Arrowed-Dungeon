using Animations;
using DG.Tweening;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Melee Auto Attack")]
    public class MeleeAutoAttack : AbilityBase
    {
        public SoundClip[] soundEffect;
        
        private Enemy.Enemy _enemy;

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy.Enemy>();
        }

        public override void Activate(GameObject target)
        {
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0;
            _enemy.agentController.agent.ResetPath();
            _enemy.transform.DOLookAt(_enemy.player.transform.position, .4f);
            _enemy.animator.SetTrigger(AnimationParameters.Attack);
            
            AudioManager.Instance.PlayRandomSoundFXClip(soundEffect, _enemy.transform);
        }

        public override void BeginCooldown()
        {
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
            _enemy.castingAbility = false;
        }
    }
}
