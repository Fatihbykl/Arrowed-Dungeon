using System;
using System.Collections;
using dynamicscroll;
using UI;
using UnityEngine;

namespace Gameplay.CraftSystem.DynamicScroll
{
    public class ScrollInitializer : MonoBehaviour
    {
        public CraftingRecipe[] recipes;
        public RarityColorsFramesInfo rarityInfo;
        public DynamicScrollRect verticalScroll;
        public GameObject referenceObject;

        private RecipeData[] _data;
        private DynamicScroll<RecipeData, DynamicRecipeObject> _verticalDynamicScroll;
        
        public void Start()
        {
            _verticalDynamicScroll = new DynamicScroll<RecipeData, DynamicRecipeObject>();
            _data = new RecipeData[recipes.Length];
            for (int i = 0; i < recipes.Length; i++)
            {
                _data[i] = new RecipeData(recipes[i], rarityInfo);
            }
            
            _verticalDynamicScroll.spacing = 25f;
            _verticalDynamicScroll.Initiate(verticalScroll, _data, 0, referenceObject);
            
            Debug.Log("Example Vertical Data : " +  _verticalDynamicScroll.RawDataList.Count);
        }
    }
}
