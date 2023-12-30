using UnityEngine;

namespace FSM
{
    public static class AnimationParameters
    {
        public static readonly int Moving = Animator.StringToHash("Moving");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int AttackStart = Animator.StringToHash("AttackStart");
        public static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        public static readonly int[] AttackCombos = new []
        {
            Animator.StringToHash("ComboAttack1"),
            Animator.StringToHash("ComboAttack2"),
            Animator.StringToHash("ComboAttack3")
        };
        
        // enemy parameters
        public static readonly int Patrol = Animator.StringToHash("Patrol");
        public static readonly int Chase = Animator.StringToHash("Chase");
        public static readonly int Defend = Animator.StringToHash("Defend");
        public static readonly int Die = Animator.StringToHash("Die");
    }
}
