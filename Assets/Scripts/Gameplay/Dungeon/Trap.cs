using System;
using Cysharp.Threading.Tasks;
using FSM;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using UnityEngine;

namespace Gameplay.Dungeon
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private int trapDamage;
        [SerializeField] private bool needTrigger = false;
        [SerializeField] private float animSpeed = 1;
        [SerializeField] private float loopWaitTime;
        [SerializeField] private float timeBetweenAnims;
        [SerializeField] private Animator animator;
        private bool animCanStart = true;

        private void Start()
        {
            animator.speed = animSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageableObject = other.gameObject.GetComponent<IDamageable>();
            damageableObject.TakeDamage(trapDamage);
        }

        private void Update()
        {
            if (!needTrigger || animator == null || !animCanStart) { return; }
            StartAnimation();
        }

        private async void StartAnimation()
        {
            animCanStart = false;
            
            animator.SetTrigger(AnimationParameters.Open);
            await UniTask.WaitForSeconds(timeBetweenAnims);
            animator.SetTrigger(AnimationParameters.Close);
            await UniTask.WaitForSeconds(timeBetweenAnims);
            await UniTask.WaitForSeconds(loopWaitTime);
            
            animCanStart = true;
        }
    }
}
