using TabSystemUI;
public class UpgradePanelTabGroup : TabGroup
{
    private UpgradeDescriptionUI upgradeDescriptionUI;
    private void Awake()
    {
        upgradeDescriptionUI = FindAnyObjectByType<UpgradeDescriptionUI>();
    }
    protected override void TabSelectAction(TabSystemUI.TabButton tabButton)
    {
        if(tabButton is UpgradeCardUI card)
        {
            upgradeDescriptionUI.Init(card.type);
        }
    }
}
