using System;
using TabSystemUI;
using UnityEngine;
public class UpgradePanelTabGroup : TabGroup
{
    [SerializeField] private UpgradeDescriptionUI upgradeDescriptionUI;
    protected override void TabSelectAction(TabSystemUI.TabButton tabButton)
    {
        if(tabButton is UpgradeCardUI card)
        {
            upgradeDescriptionUI.Init(card.type);
        }
    }
}
