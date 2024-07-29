using System;
using System.Collections.Generic;
using UnityEngine;

namespace TabSystemUI
{
    public class TabGroup : MonoBehaviour
    {
        public List<GameObject> pages;
        public Color tabIdel;
        public Color tabHover;
        public Color tabActive;
        public AudioClip selectTabSound;

        [SerializeField] private List<TabButton> buttons = new List<TabButton>();
        [SerializeField] private TabButton selectedTab;

        public void Add(TabButton tabButton)
        {
            buttons.Add(tabButton);
            if(selectedTab == null)
            {
                OnTabSelected(tabButton);
            }
        }
        public void OnTabEnter(TabButton tabButton)
        {
            ResetTabs();
            if (selectedTab == null || tabButton != selectedTab)
            {
                tabButton.SetBGColor(tabHover);
            }
        }
        public void OnTabExit(TabButton tabButton)
        {

            if (tabButton != selectedTab) { tabButton.SetBGColor(tabIdel); }

        }
        public void OnTabSelected(TabButton tabButton)
        {
            if (tabButton != selectedTab) 
            {
                selectedTab = tabButton;
                ResetTabs();
                tabButton.SetBGColor(tabActive);
                SoundManager.instance.PlaySoundClip(selectTabSound, transform, 1f);
                TabSelectAction(tabButton);
            }
        }

        protected virtual void TabSelectAction(TabButton tabButton)
        {
            int index = tabButton.transform.GetSiblingIndex();
            for (int i = 0; i < pages.Count; i++)
            {
                if (i == index)
                {
                    pages[i].SetActive(true);
                }
                else
                {
                    pages[i].SetActive(false);
                }
            }
        }

        public void ResetTabs()
        {
            foreach (var button in buttons)
            {
                if(button == selectedTab) { continue; }
                button.SetBGColor(tabIdel);
            }
        }
    }
}