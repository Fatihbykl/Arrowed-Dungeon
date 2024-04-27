using UnityEngine;

namespace FSM
{
    public static class AnimationParameters
    {
        // player parameters
        public static readonly int Moving = Animator.StringToHash("Moving");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int AttackMode = Animator.StringToHash("AttackMode");
        public static readonly int Strafe = Animator.StringToHash("Strafe");
        public static readonly int EquipBow = Animator.StringToHash("EquipBow");
        public static readonly int DisarmBow = Animator.StringToHash("DisarmBow");
        
        // enemy parameters
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int Defend = Animator.StringToHash("Defend");
        public static readonly int CanSpin = Animator.StringToHash("CanSpin");
        public static readonly int JumpAttack = Animator.StringToHash("JumpAttack");
        public static readonly int LineAttack = Animator.StringToHash("LineAttack");
        public static readonly int HealSkill = Animator.StringToHash("HealSkill");
        
        // player and enemy shared parameters
        public static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        public static readonly int Die = Animator.StringToHash("Die");
        
        // dungeon parameters
        public static readonly int Open = Animator.StringToHash("Open");
        public static readonly int Close = Animator.StringToHash("Close");
    }
}
