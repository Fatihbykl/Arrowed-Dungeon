using System;
using System.Linq;
using AbilitySystem.NPC;
using Gameplay.Enemy;
using UnityEngine;
using UnityEngine.Animations;

namespace Behaviours
{
    public class JumpAttackBehaviour : StateMachineBehaviour
    {
        private Enemy _enemy;
        private JumpAttack _jumpAttack;
        private float start, end;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemy = animator.gameObject.GetComponent<Enemy>();
            //
            // var startPoint = animator.gameObject.transform.position;
            // var direction = animator.gameObject.transform.forward;
            // var endPoint = startPoint + direction * 3f;
            // endPoint = _enemy.player.transform.position;
            //
            // _enemy.agentController.speed = 3f;
            // _enemy.agentController.agent.SetDestination(endPoint);

            _jumpAttack =
                _enemy.abilityHolders.FirstOrDefault(a => String.Equals(a.ability.name, "JumpAttack(Clone)"))
                    ?.ability as JumpAttack;

            if (_jumpAttack != null) _jumpAttack.OnAnimationJump();
            else
            {
                Debug.LogError("JumpAttack(Clone) not found!");
            }

            start = Time.time;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            end = Time.time;
            _jumpAttack.OnAnimationLand();
        }
    }
}