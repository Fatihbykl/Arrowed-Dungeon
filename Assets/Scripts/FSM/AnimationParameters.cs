using UnityEngine;

namespace FSM
{
    public static class AnimationParameters
    {
        public static readonly int Moving = Animator.StringToHash("Moving");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int AttackStart = Animator.StringToHash("AttackStart");
        public static readonly int[] AttackCombos = new []
        {
            Animator.StringToHash("ComboAttack1"),
            Animator.StringToHash("ComboAttack2")
        };
    }
}
