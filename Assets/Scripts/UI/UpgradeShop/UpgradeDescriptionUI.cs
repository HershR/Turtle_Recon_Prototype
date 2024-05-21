using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeDescriptionUI : MonoBehaviour
{

    [SerializeField] private PlayerStatsSO playerStatsSO;

    [SerializeField] private TextMeshProUGUI totalTokensText;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI upgradeLevelText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    [SerializeField] private Slider upgradeLevelSlider;
    [SerializeField] private Button upgradePurchaceButton;

    private StatType statType;

    private void Start()
    {
        upgradePurchaceButton.onClick.AddListener(BuyUpgrade);
        totalTokensText.text = "Tokens: " + playerStatsSO.Tokens.ToString();
        Init(StatType.Health);
    }
    public void Init(StatType type)
    {

        statType = type;
        StatSO stat = playerStatsSO.GetStat(type);
        upgradeNameText.text = stat.Name;
        upgradeDescriptionText.text = stat.GetDescription();
        upgradeLevelText.text = "Lvl: " + stat.Level.ToString();
        upgradeLevelSlider.value = stat.Level / (float)stat.MaxLevel;
        upgradeCostText.text = stat.GetCost() >= 0 ? stat.GetCost().ToString() : "Max";
        upgradePurchaceButton.interactable = playerStatsSO.IsUpgradable(type);
    }

    public void Refresh()
    {
        totalTokensText.text = "Tokens: " + playerStatsSO.Tokens.ToString();
        StatSO stat = playerStatsSO.GetStat(statType);
        upgradeDescriptionText.text = stat.GetDescription();
        upgradeLevelText.text = "Lvl: " + stat.Level.ToString();
        upgradeLevelSlider.value = stat.Level / (float)stat.MaxLevel;
        upgradeCostText.text = stat.GetCost() >= 0 ? stat.GetCost().ToString() : "Max";
        upgradePurchaceButton.interactable = playerStatsSO.IsUpgradable(statType);
    }
    private void BuyUpgrade()
    {
        Debug.Log($"Buy Upgrade: {System.Enum.GetName(typeof(StatType), statType)}");
        playerStatsSO.UpgradeStat(statType);
        Refresh();
    }


}
