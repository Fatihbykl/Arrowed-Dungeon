using System;
using System.Collections.Generic;
using Gameplay.InventorySystem;
using Gameplay.StatSystem;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerStats : MonoBehaviour
    {
        public IntegerStat maxHealth;
        public IntegerStat damage;
        public FloatStat armor;
        public FloatStat missChance; // stability
        public FloatStat runningSpeed;
        public FloatStat walkingSpeed;
        public FloatStat attackCooldown;

        [HideInInspector] public VitalStat health;

        private void Start()
        {
            Inventory.Instance.EquipmentChanged += OnEquipmentChanged;
        }

        public void InitHealth()
        {
            health.BaseValue = maxHealth.BaseValue;
            health.useUpperBound = true;
            health.upperBound = maxHealth.BaseValue;
        }

        public void RemoveAllModifiersFromStats()
        {
            maxHealth.RemoveAllModifiers();
            damage.RemoveAllModifiers();
            armor.RemoveAllModifiers();
            missChance.RemoveAllModifiers();
            runningSpeed.RemoveAllModifiers();
            walkingSpeed.RemoveAllModifiers();
            attackCooldown.RemoveAllModifiers();
        }

        public void UpdateStatsWithEquipments(List<InventorySlot> equipments)
        {
            foreach (var slot in equipments)
            {
                if (slot == null) { continue; }
                
                foreach (var modifier in slot.item.modifiers)
                {
                    if (modifier == null) { continue; }
                    
                    switch (modifier.TargetStat)
                    {
                        case StatID.MaxHealth:
                            maxHealth.AddModifier(modifier);
                            break;
                        case StatID.Damage:
                            damage.AddModifier(modifier);
                            break;
                        case StatID.Armor:
                            armor.AddModifier(modifier);
                            break;
                        case StatID.MissChance:
                            missChance.AddModifier(modifier);
                            break;
                        case StatID.RunningSpeed:
                            runningSpeed.AddModifier(modifier);
                            break;
                        case StatID.WalkingSpeed:
                            walkingSpeed.AddModifier(modifier);
                            break;
                        case StatID.AttackCooldown:
                            attackCooldown.AddModifier(modifier);
                            break;
                        case StatID.None:
                            break;
                    }
                }
            }
        }

        private void OnEquipmentChanged()
        {
            RemoveAllModifiersFromStats();
            UpdateStatsWithEquipments(Inventory.Instance.equipmentSlots);
        }
    }
}