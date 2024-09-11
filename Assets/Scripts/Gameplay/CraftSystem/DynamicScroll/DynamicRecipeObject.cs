using System;
using dynamicscroll;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.CraftSystem.DynamicScroll
{
    public class DynamicRecipeObject : DynamicScrollObject<RecipeData>
    {
        public override float CurrentHeight { get; set; }
        public override float CurrentWidth { get; set; }
        
        private Image _frame;
        private Image _icon;
        private TextMeshProUGUI _title;
        private Button _button;
        private RecipeData _data;

        private void Awake()
        {
            CurrentHeight = GetComponent<RectTransform>().rect.height;
            CurrentWidth = GetComponent<RectTransform>().rect.width;
            
            _frame = transform.Find("Frame").GetComponent<Image>();
            _icon = _frame.transform.Find("Icon").GetComponent<Image>();
            _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnRecipeClicked);
        }

        public void OnRecipeClicked()
        {
            EventManager.EmitEvent(EventStrings.RecipeClicked, _data);
        }

        public override void UpdateScrollObject(RecipeData item, int index)
        {
            base.UpdateScrollObject(item, index);

            _data = item;
            _frame.sprite = item.rarityInfo.rarityFrames[(int)item.recipe.result.item.itemRarity];
            _icon.sprite = item.recipe.result.item.icon;
            _title.text = item.recipe.result.item.title;
        }

        public override void SetPositionInViewport(Vector2 position, Vector2 distanceFromCenter)
        {
            position.x += 200;
            base.SetPositionInViewport(position, distanceFromCenter);
            
        }
    }
}
