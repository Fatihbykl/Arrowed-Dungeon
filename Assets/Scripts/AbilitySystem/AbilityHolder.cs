using System;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityHolder : MonoBehaviour
    {
        public enum AbilityState { Ready, Cooldown, Casting }

        public AbilityBase ability;
        public GameObject target;
        public AbilityState currentState = AbilityState.Ready;

        public float _cooldownTimer;
        private float _castTimer;

        private void Start()
        {
            currentState = AbilityState.Ready;
        }

        private void Update()
        {
            if (currentState == AbilityState.Casting)
            {
                if (_castTimer > 0)
                {
                    _castTimer -= Time.deltaTime;
                }
                else
                {
                    ability.BeginCooldown();
                    currentState = AbilityState.Cooldown;
                    _cooldownTimer = ability.cooldown;
                }
            }
            else if (currentState == AbilityState.Cooldown)
            {
                if (_cooldownTimer > 0)
                {
                    _cooldownTimer -= Time.deltaTime;
                }
                else
                {
                    if (ability.IsReady())
                    {
                        currentState = AbilityState.Ready;
                    }
                    else
                    {
                        _cooldownTimer = 0.5f;
                    }
                }
            }
        }

        public void ActivateAbility(Vector2? direction = null)
        {
            
            if (currentState != AbilityState.Ready) { return; }

            if (direction == null) { ability.Activate(target); }
            else { ability.Activate(target, direction); }
            
            currentState = AbilityState.Casting;
            _castTimer = ability.castTime;
        }
    }
}
