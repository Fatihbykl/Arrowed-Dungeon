using System;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityHolder : MonoBehaviour
    {
        public AbilityBase ability;
        public GameObject owner;
        public GameObject target;

        private float cooldownTimer;
        private float castTimer;

        private void Start()
        {
            ability.currentState = AbilityBase.AbilityState.Ready;
        }

        private void Update()
        {
            if (ability.currentState == AbilityBase.AbilityState.Casting)
            {
                if (castTimer > 0)
                {
                    castTimer -= Time.deltaTime;
                }
                else
                {
                    ability.BeginCooldown(owner, target);
                    ability.currentState = AbilityBase.AbilityState.Cooldown;
                    cooldownTimer = ability.cooldown;
                }
            }
            else if (ability.currentState == AbilityBase.AbilityState.Cooldown)
            {
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
                }
                else
                {
                    ability.currentState = AbilityBase.AbilityState.Ready;
                }
            }
        }

        public void ActivateAbility()
        {
            if (ability.currentState != AbilityBase.AbilityState.Ready) { return; }

            ability.Activate(owner, target);
            ability.currentState = AbilityBase.AbilityState.Casting;
            castTimer = ability.castTime;
        }
    }
}
