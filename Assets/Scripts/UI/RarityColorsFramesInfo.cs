using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Custom/Info/Rarity Colors And Frames")]
    public class RarityColorsFramesInfo : ScriptableObject
    {
        [InfoBox("Place in order! 0=Common, 1=Rare, 2=Epic, 3=Legendary, 4=Mythic, 5=None")]
        [Space(10)]
        public Sprite[] rarityFrames;
        public Color[] rarityColors;
     }
}
