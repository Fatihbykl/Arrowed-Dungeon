using System.Linq;
using Cysharp.Threading.Tasks;
using FSM;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using Managers;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Heal Circle")]
    public class HealCircleSkill : AbilityBase
    {
        public GameObject healParticle;
        public LayerMask mask;
        public float circleSize;
        public float healInterval;
        public float healDuration;
        public int healAmountEveryInterval;

        private Enemy _enemy;
        private Enemy _lowHpEnemy;
        private Vector3 _healCirclePosition;
        private GameObject _particle;
        private float _circleSize;

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy>();
        }

        public override void Activate(GameObject target)
        {
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _circleSize = circleSize / 4; // adjustment for match visual effect with overlap sphere
            _enemy.animator.SetTrigger(AnimationParameters.HealSkill);

            CreateHealCircle();
            HealInterval();
        }

        private void CreateHealCircle()
        {
            if (!_lowHpEnemy) { return; }

            _healCirclePosition = _lowHpEnemy.transform.position;
            
            _particle = Instantiate(healParticle);
            _particle.transform.localScale = new Vector3(_circleSize, _circleSize, _circleSize);
            _particle.transform.position = _healCirclePosition;
        }

        private async void HealInterval()
        {
            if (!_lowHpEnemy) { return; }
            
            for (int i = 0; i < healDuration / healInterval; i++)
            {
                Collider[] colliders = Physics.OverlapSphere(_healCirclePosition, circleSize, mask);
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
            _lowHpEnemy = AIManager.Instance.Units
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