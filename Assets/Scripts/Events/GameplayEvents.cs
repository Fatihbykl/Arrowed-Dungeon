using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayEvents
{
    public static Action<string, int> ArrowDead;
    public static Action<int, int> KeyCollected;
    public static Action<int> PlayerGetDamaged;
    public static Action<float> FreezeSkillActivated;
    public static Action<int> DestroyerSkillActivated;
}
