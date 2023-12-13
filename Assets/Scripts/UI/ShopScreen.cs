using ShopSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static ShopSystem.ItemData;

public class ShopScreen : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private ShopSystem.ShopItemSO upgrades, skills;

    [SerializeField]
    private VisualTreeAsset shopItemTree;

    private Label coinText;
    private int totalCoin;
    private VisualElement upgradesCol, skillsCol;

    private void OnEnable()
    {

        UIDocument document = GetComponent<UIDocument>();

        coinText = document.rootVisualElement.Q<Label>("coinText");
        upgradesCol = document.rootVisualElement.Q<VisualElement>("upgrades");
        skillsCol = document.rootVisualElement.Q<VisualElement>("skills");

        AddShopItems(upgrades, upgradesCol);
        AddShopItems(skills, skillsCol);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void AddShopItems(ShopSystem.ShopItemSO items, VisualElement root)
    {
        for (int index = 0; index < items.itemData.Length; index++)
        {
            TemplateContainer itemRow = shopItemTree.Instantiate();
            VisualElement icon = itemRow.Q<VisualElement>("icon");
            Label title = itemRow.Q<Label>("title");
            Label levelInfo = itemRow.Q<Label>("level-info");
            Label desc = itemRow.Q<Label>("desc");
            Label cost = itemRow.Q<Label>("cost");
            Button button = itemRow.Q<Button>("upgradeButton");

            var data = items.itemData[index];

            button.RegisterCallback<ClickEvent>(OnBuyButtonClicked);
            button.userData = data;
            
            icon.style.backgroundImage = new StyleBackground(data.icon);
            title.text = data.title.ToString();
            desc.text = data.description;
            cost.text = data.getCurrentUpgradeData().cost.ToString();

            if (data.category == ShopSystem.ItemData.ShopItemCategory.Upgrades) 
            {
                levelInfo.text = $"{data.currentLevel + 1} -> {data.currentLevel + 2}";
            }
            else { levelInfo.text = "1"; }

            root.Add(itemRow);
        }
    }

    private void OnBuyButtonClicked(ClickEvent evt)
    {
        var button = evt.target as Button;
        ShopSystem.ItemData item = button.userData as ShopSystem.ItemData;
        var cost = item.getCurrentUpgradeData().cost;

        TryToBuyItem(item, cost);
    }

    private void TryToBuyItem(ItemData item, int cost)
    {
        if (totalCoin >= cost)
        {
            Debug.Log("Aldin");
            totalCoin -= cost;
            item.BuyItem();
        }
        else
        {
            Debug.Log("alamadin");
        }
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
