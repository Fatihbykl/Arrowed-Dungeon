using System;
using System.Linq;
using AbilitySystem.NPC;
using Gameplay.Enemy;
using UnityEngine;

namespace Behaviours
{
    public class LineAttackBehaviour : StateMachineBehaviour
    {
        private Enemy _enemy;
        private LineAttack _lineAttack;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemy = animator.gameObject.GetComponent<Enemy>();
            
            _lineAttack =
                _enemy.abilityHolders.FirstOrDefault(a => String.Equals(a.ability.name, "LineAttack(Clone)"))
                    ?.ability as LineAttack;

            if (_lineAttack != null) _lineAttack.OnHitGround();
            else
            {
                Debug.LogError("LineAttack(Clone) not found!");
            }
        }
    }
}
