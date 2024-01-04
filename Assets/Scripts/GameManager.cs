using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public GameObject keysObject;
    public GameObject gate;
    public int currentLevelCoin = 0;
    public int currentLevelBrokenArrows = 0;
    public int collectedKeyCount = 0;
    public int totalKeyCount = 0;
    public int currentLevel = 0;

    public int playerBaseHealth { get; private set; } = 1;
    public float playerSpeed { get; private set; } = 4;
    public int playerShield { get; private set; } = 1;

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Manager in the scene.");
        }
        instance = this;
    }

    private void OnEnable()
    {
        if (keysObject != null) { totalKeyCount = keysObject.transform.childCount; }

        GameplayEvents.ArrowDead += onArrowDead;
        ShopEvents.ItemUpgraded += OnItemUpgraded;
    }

    private void OnDisable()
    {
        GameplayEvents.ArrowDead -= onArrowDead;
        ShopEvents.ItemUpgraded -= OnItemUpgraded;
    }

    public void CollectedKey()
    {
        collectedKeyCount++;
        if (collectedKeyCount >= totalKeyCount)
        {
            gate.GetComponent<Animator>().Play("Open");
        }
    }

    public void onArrowDead(string arrowType, int coinReward)
    {
        currentLevelCoin += coinReward;
        currentLevelBrokenArrows++;
    }

    private void OnItemUpgraded(ShopBaseSO item)
    {
        if (item.title == ItemTitle.Health) { playerBaseHealth = (int)item.GetCurrentStat(); }
        else if (item.title == ItemTitle.Shield) { playerShield = (int)item.GetCurrentStat(); }
        else if (item.title == ItemTitle.Speed) { playerSpeed = item.GetCurrentStat(); }
    }

    public void LoadData(GameData data)
    {
        currentLevel = data.currentLevel;
        playerBaseHealth = data.health;
        playerSpeed = data.speed;
        playerShield = data.shield;
    }

    public void SaveData(GameData data)
    {
        data.currentLevel = currentLevel;
        data.coins += currentLevelCoin;
        data.health = playerBaseHealth;
        data.speed = playerSpeed;
        data.shield = playerShield;
    }
}
