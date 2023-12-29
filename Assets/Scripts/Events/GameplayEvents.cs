using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Player;
using Gameplay.Player.DamageDealers;
using UnityEngine;

public static class GameplayEvents
{
    public static Action<string, int> ArrowDead;
    public static Action<int> PlayerGetDamaged;
    public static Action<int> AttackTransition;
    public static Action<GameObject> StartDealDamage;
    public static Action<GameObject> EndDealDamage;

    public static Action<int, int, GameObject, GameObject> KeyCollected;
    public static Action KeyAnimationFinished;
    public static Action LevelPassed;
    public static Action LevelFailed;

    public static Action<float> FreezeSkillActivated;
    public static Action<int> DestroyerSkillActivated;
}
