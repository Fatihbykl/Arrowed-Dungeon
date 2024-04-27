using System;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityHolder : MonoBehaviour
    {
        public enum AbilityState { Ready, Cooldown, Casting }

        public AbilityBase ability;
        public GameObject owner;
        public GameObject target;
        public AbilityState currentState = AbilityState.Ready;

        private float cooldownTimer;
        private float castTimer;

        private void Start()
        {
            currentState = AbilityState.Ready;
        }

        private void Update()
        {
            if (currentState == AbilityState.Casting)
            {
                if (castTimer > 0)
                {
                    castTimer -= Time.deltaTime;
                }
                else
                {
                    ability.BeginCooldown();
                    currentState = AbilityState.Cooldown;
                    cooldownTimer = ability.cooldown;
                }
            }
            else if (currentState == AbilityState.Cooldown)
            {
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
                }
                else
                {
                    if (ability.IsReady())
                    {
                        currentState = AbilityState.Ready;
                    }
                    else
                    {
                        cooldownTimer = 0.5f;
                    }
                }
            }
        }

        public void ActivateAbility()
        {
            if (currentState != AbilityState.Ready) { return; }

            ability.Activate(owner, target);
            currentState = AbilityState.Casting;
            castTimer = ability.castTime;
        }
    }
}
