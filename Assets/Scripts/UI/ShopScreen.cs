using System;
using DataPersistance;
using DataPersistance.Data;
using DataPersistance.Data.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class ShopScreen : MonoBehaviour, IDataPersistence
    {
        [SerializeField]
        private UpgradesSO[] upgrades;

        [SerializeField]
        private SkillsSO[] skills;

        [SerializeField]
        private VisualTreeAsset shopItemTree;

        private UIDocument document;
        private Label coinText;
        private int totalCoin;
        private VisualElement upgradesCol, skillsCol;

        private void OnEnable()
        {

            document = GetComponent<UIDocument>();

            coinText = document.rootVisualElement.Q<Label>("coinText");
            upgradesCol = document.rootVisualElement.Q<VisualElement>("upgrades");
            skillsCol = document.rootVisualElement.Q<VisualElement>("skills");

            AddShopItems(upgrades, upgradesCol);
            AddShopItems(skills, skillsCol);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    
        private void AddShopItems(ShopBaseSO[] items, VisualElement root)
        {
            foreach (var item in items)
            {
                TemplateContainer itemRow = shopItemTree.Instantiate();
                VisualElement icon = itemRow.Q<VisualElement>("icon");
                Label title = itemRow.Q<Label>("title");
                Label levelInfo = itemRow.Q<Label>("level-info");
                Label desc = itemRow.Q<Label>("desc");
                Label cost = itemRow.Q<Label>("cost");
                Button button = itemRow.Q<Button>("upgradeButton");

                levelInfo.name = $"levelInfo_{item.title}";
                cost.name = $"cost_{item.title}";

                button.RegisterCallback<ClickEvent>(OnBuyButtonClicked);
                button.userData = item;

                icon.style.backgroundImage = new StyleBackground(item.icon);
                title.text = item.title.ToString();
                desc.text = item.description;
                cost.text = item.GetCost();
                levelInfo.text = item.GetTitleInfoText();

                if (item.GetCost() == "MAX") { button.SetEnabled(false); }
                root.Add(itemRow);
            }
        }
    

        private void OnBuyButtonClicked(ClickEvent evt)
        {
            var button = evt.target as Button;
            ShopBaseSO item = button.userData as ShopBaseSO;
            if (item.GetCost() != "MAX") 
            {
                var cost = Int32.Parse(item.GetCost());

                var isCompleted = TryToBuyItem(item, cost);

                if (isCompleted)
                {
                    UpdateUI(item, button);
                }
                else { }
            }
            else
            {
                button.SetEnabled(false);
            }
        
        }
    
        private bool TryToBuyItem(ShopBaseSO item, int cost)
        {
            if (totalCoin >= cost)
            {
                Debug.Log("Aldin");
                totalCoin -= cost;
                item.BuyItem();

                return true;
            }
            else
            {
                Debug.Log("alamadin");
                return false;
            }
        }

        private void UpdateUI(ShopBaseSO item, Button button)
        {

            Label levelInfo = document.rootVisualElement.Q<Label>($"levelInfo_{item.title}");
            Label cost = document.rootVisualElement.Q<Label>($"cost_{item.title}");

            levelInfo.text = item.GetTitleInfoText();
            cost.text = item.GetCost();
            coinText.text = totalCoin.ToString();
        }
    
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            coinText.text = totalCoin.ToString();
        }

        public void LoadData(GameData data)
        {
            totalCoin = data.coins;
        }

        public void SaveData(GameData data)
        {
            data.coins = totalCoin;
        }
    }
}
