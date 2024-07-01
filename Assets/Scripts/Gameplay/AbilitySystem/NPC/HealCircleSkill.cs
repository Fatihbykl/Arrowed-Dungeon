using System.Linq;
using Animations;
using Cysharp.Threading.Tasks;
using Gameplay.Interfaces;
using Gameplay.Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Heal Circle")]
    public class HealCircleSkill : AbilityBase
    {
        public GameObject healParticle;
        public LayerMask mask;
        public float circleRadius;
        public float healInterval;
        public float healDuration;
        public int healAmountEveryInterval;

        private Enemy.Enemy _enemy;
        private Enemy.Enemy _lowHpEnemy;
        private Vector3 _healCirclePosition;
        private GameObject _particle;
        private float _circleScale;

        [Header("Sound Effects")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(5)]
        public SoundClip castingSound;
        

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy.Enemy>();
        }

        public override void Activate(GameObject target)
        {
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _circleScale = circleRadius / 4; // adjustment for match visual effect with overlap sphere
            _enemy.animator.SetTrigger(AnimationParameters.HealSkill);

            AudioManager.Instance.PlaySoundFXClip(castingSound, _enemy.transform);
            CreateHealCircle();
            HealInterval();
        }

        private void CreateHealCircle()
        {
            if (!_lowHpEnemy) { return; }

            _healCirclePosition = _lowHpEnemy.transform.position;
            
            _particle = Instantiate(healParticle);
            _particle.transform.localScale = Vector3.one * _circleScale;
            _particle.transform.position = _healCirclePosition;
        }

        private async void HealInterval()
        {
            if (!_lowHpEnemy) { return; }
            
            for (int i = 0; i < healDuration / healInterval; i++)
            {
                Collider[] colliders = Physics.OverlapSphere(_healCirclePosition, circleRadius, mask);
                if (colliders.Length > 0)
                {
                    for (int j = 0; j < colliders.Length; j++)
                    {
                        if (colliders[j].TryGetComponent(out IHealable enemy))
                        {
                            enemy.Heal(healAmountEveryInterval);
                        }
                    }
                }
                await UniTask.WaitForSeconds(healInterval);
            }

            Destroy(_particle);
        }

        public override bool IsReady()
        {
            _lowHpEnemy = AIManager.Instance.units
                .Where(enemy => enemy.enemyStats.health.Value != enemy.enemyStats.health.BaseValue)
                .OrderBy(enemy => enemy.enemyStats.health.Value)
                .FirstOrDefault();
        
            if (_lowHpEnemy)
            {
                return true;
            }
        
            return false;
        }

        public override void BeginCooldown()
        {
            _enemy.castingAbility = false;
        }
    }
}