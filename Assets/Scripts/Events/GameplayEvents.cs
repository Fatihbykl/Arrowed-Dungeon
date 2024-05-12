using System;
using UnityEngine;

namespace Events
{
    public static class GameplayEvents
    {
        public static Action<string, int> ArrowDead;
        public static Action<int> PlayerGetDamaged;

        public static Action<int, int, GameObject, GameObject> KeyCollected;
        public static Action KeyAnimationFinished;
        public static Action LevelPassed;
        public static Action LevelFailed;

        public static Action<float> FreezeSkillActivated;
        public static Action<int> DestroyerSkillActivated;
    }
}
