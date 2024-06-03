using System;
using AbilitySystem;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Joystick.Joysticks
{
    public class SkillFixedJoystick : Base.Joystick
    {
        public GameObject directionalIndicator;
        public GameObject regionalIndicator;
        public SkillType skillType;
        public AbilityHolder abilityHolder;
        public Image cooldownImage;
        
        private bool _skillPressed = false;
        private GameObject _indicator;
        private Vector2 _lastDirection;
        private TextMeshProUGUI _cooldownText;
        private Vector3 _vector3;

        protected override void Start()
        {
            base.Start();

            _vector3 = new Vector3(0, 0, 0);
            _cooldownText = cooldownImage.GetComponentInChildren<TextMeshProUGUI>();
            
            if (skillType == SkillType.Directional)
            {
                _indicator = Instantiate(directionalIndicator, GameManager.instance.playerObject.transform);
                _indicator.SetActive(false);
            }

            if (skillType == SkillType.Regional)
            {
                _indicator = Instantiate(regionalIndicator, GameManager.instance.playerObject.transform);
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
            _vector3 = new Vector3(Horizontal, 0f, Vertical);
            _vector3.x = Horizontal;
            _vector3.z = Vertical;
            _indicator.SetActive(true);
            _indicator.transform.rotation = Quaternion.FromToRotation(Vector3.back, _vector3);
            _lastDirection = Direction;
        }
        
        private void RegionalSkillLoop()
        {
            _vector3.x = Horizontal;
            _vector3.z = Vertical;
            _indicator.SetActive(true);
            
            _indicator.transform.Translate(_vector3 * Time.deltaTime * 10f, Space.World);
            _indicator.transform.localPosition = Vector3.ClampMagnitude(_indicator.transform.localPosition, 3f);
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
                abilityHolder.ActivateAbility(Vector3.zero);
            }
            else if (skillType == SkillType.Regional)
            {
                abilityHolder.ActivateAbility(_indicator.transform.position);
            }
            else
            {
                var direction = new Vector3(_lastDirection.x, 0, _lastDirection.y);
                abilityHolder.ActivateAbility(direction);
            }
            
            if (_indicator != null) { _indicator.SetActive(false); }
        }
    }
}