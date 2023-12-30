﻿using UnityEngine;

namespace Gameplay.Player.DamageDealers
{
    public interface IDamageable
    {
        public void StartDealDamage();
        public void EndDealDamage();
        public void TakeDamage(int damage);
        public void StartTakeDamageAnim();
    }
}