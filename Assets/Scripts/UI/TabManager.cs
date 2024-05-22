using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TabManager : MonoBehaviour
    {
        public GameObject[] tabs;
        public Image[] tabButtons;
        public Sprite inactiveTabBackground, activeTabBackground;

        public void SwitchToTab(int tabID)
        {
            foreach (var tab in tabs)
            {
                tab.SetActive(false);
            }
            tabs[tabID].SetActive(true);

            foreach (Image button in tabButtons)
            {
                button.sprite = inactiveTabBackground;
            }
            tabButtons[tabID].sprite = activeTabBackground;
        }
    }
}
