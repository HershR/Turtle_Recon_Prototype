using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeDescriptionUI : MonoBehaviour
{

    [SerializeField] private PlayerStatsSO playerStatsSO;

    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    
    [SerializeField] private Slider upgradeLevelSlider;
    [SerializeField] private Button upgradePurchaceButton;

    private StatType statType;

    private void Start()
    {
        upgradePurchaceButton.onClick.AddListener(BuyUpgrade);
    }
    public void Init(StatType type)
    {

        statType = type;
        StatSO stat = playerStatsSO.GetStat(type);
        upgradeName.text = stat.Name;
        upgradeDescription.text = stat.GetDescription();
        upgradeLevelSlider.value = stat.Level / stat.MaxLevel;
        upgradeCost.text = stat.GetCost().ToString();
        upgradePurchaceButton.interactable = playerStatsSO.IsUpgradable(type);
    }

    public void Refresh()
    {
        StatSO stat = playerStatsSO.GetStat(statType);
        //upgradeName.text = stat.Name;
        upgradeDescription.text = stat.GetDescription();
        upgradeLevelSlider.value = stat.Level / stat.MaxLevel;
        upgradeCost.text = stat.GetCost().ToString();
        upgradePurchaceButton.interactable = playerStatsSO.IsUpgradable(statType);
    }
    private void BuyUpgrade()
    {
        Debug.Log($"Buy Upgrade: {System.Enum.GetName(typeof(StatType), statType)}");
        playerStatsSO.UpgradeStat(statType);
        Refresh();
    }


}
