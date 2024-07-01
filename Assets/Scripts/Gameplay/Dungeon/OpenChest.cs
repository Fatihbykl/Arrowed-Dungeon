using System;
using Animations;
using FSM;
using UnityEngine;

namespace Gameplay.Dungeon
{
    public class OpenChest : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private bool isOpened = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (isOpened) { return; }
            animator.SetTrigger(AnimationParameters.Open);
            isOpened = true;
        }
    }
}
