using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coins;
    public int currentLevel;

    // Stats
    public int health;
    public int shield;
    public int speed;

    // Skills
    public int immortal;
    public int freeze;
    public int destroyer;

    public float musicVolume;
    public float sfxVolume;

    public GameData()
    {
        this.coins = 0;
        this.currentLevel = 0;
        this.health = 0;
        this.shield = 0;
        this.speed = 0;
        this.immortal = 0;
        this.freeze = 0;
        this.destroyer = 0;
        this.musicVolume = 0;
        this.sfxVolume = 0;
    }
}
