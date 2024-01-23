using UnityEngine;

namespace FSM.Enemy
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        TakeDamage,
        Die,
        Spin
    }
}