using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public int totalCoins;
    public int earnedCoins;

    private void Start()
    {
        totalCoins = 0;
        earnedCoins = 0;
    }

    public void UpdateEarnedCoins(int earnedCoins)
    {
        if(earnedCoins > 0) { this.earnedCoins += earnedCoins; }
    }

    public void UpdateTotalCoins()
    {
        totalCoins += earnedCoins;
    }
}
