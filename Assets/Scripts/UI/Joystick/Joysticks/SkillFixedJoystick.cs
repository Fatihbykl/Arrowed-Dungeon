using System;
using AbilitySystem;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Joystick.Joysticks
{
    public class SkillFixedJoystick : Base.Joystick
    {
        public GameObject indicator;
        public SkillType skillType;
        public AbilityHolder abilityHolder;
        public Image cooldownImage;
        
        private bool _skillPressed = false;
        private GameObject _indicator;
        private Vector2 _lastDirection;
        private TextMeshProUGUI _cooldownText;

        protected override void Start()
        {
            base.Start();

            _cooldownText = cooldownImage.GetComponentInChildren<TextMeshProUGUI>();
            
            if (skillType != SkillType.NonTargetable)
            {
                _indicator = Instantiate(indicator, GameManager.instance.playerObject.transform);
                _indicator.SetActive(false);
            }
        }

        private void Update()
        {
            if (abilityHolder.currentState == AbilityHolder.AbilityState.Cooldown)
            {
                cooldownImage.fillAmount = abilityHolder._cooldownTimer / abilityHolder.ability.cooldown;
                _cooldownText.text = ((int)abilityHolder._cooldownTimer + 1).ToString();
            }

            if (abilityHolder.currentState == AbilityHolder.AbilityState.Ready)
            {
                _cooldownText.text = "";
            }
            if (!_skillPressed) { return; }

            switch (skillType)
            {
                case SkillType.Directional:
                    DirectionalSkillLoop();
                    break;
                case SkillType.Regional:
                    RegionalSkillLoop();
                    break;
            }
        }

        private void DirectionalSkillLoop()
        {
            _indicator.SetActive(true);
            _indicator.transform.rotation = Quaternion.FromToRotation(Vector3.back, new Vector3(Horizontal, 0f , Vertical));
            _lastDirection = Direction;
        }
        
        private void RegionalSkillLoop()
        {
            
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            _skillPressed = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            _skillPressed = false;
            Debug.Log(_lastDirection);

            if (skillType == SkillType.NonTargetable)
            {
                abilityHolder.ActivateAbility();
            }
            else
            {
                abilityHolder.ActivateAbility(_lastDirection);
            }
            
            if (_indicator != null) { _indicator.SetActive(false); }
        }
    }
}