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
    [SerializeField] private AudioClip BuySound;

    private StatType statType;

    private void Start()
    {
        upgradePurchaceButton.onClick.AddListener(BuyUpgrade);
        totalTokensText.text = playerStatsSO.Tokens.ToString();
        Init(StatType.Health);
    }
    public void Init(StatType type)
    {

        statType = type;
        StatSO stat = playerStatsSO.GetStat(type);
        upgradeNameText.text = stat.Name;
        Refresh();
    }

    public void Refresh()
    {
        totalTokensText.text = playerStatsSO.Tokens.ToString();
        StatSO stat = playerStatsSO.GetStat(statType);
        upgradeDescriptionText.text = !stat.IsMaxLevel() ? stat.GetDescription() : "Completed";
        upgradeLevelText.text = "Lvl: " + stat.Level.ToString();
        upgradeLevelSlider.value = stat.Level / (float)stat.MaxLevel;
        upgradeCostText.text = !stat.IsMaxLevel() ? stat.GetCost().ToString() : "Max";
        upgradePurchaceButton.interactable = playerStatsSO.IsUpgradable(statType);
    }
    private void BuyUpgrade()
    {
        Debug.Log($"Buy Upgrade: {System.Enum.GetName(typeof(StatType), statType)}");
        SoundManager.instance.PlaySoundClip(BuySound, transform, 1f);
        playerStatsSO.UpgradeStat(statType);
        Refresh();
    }


}
