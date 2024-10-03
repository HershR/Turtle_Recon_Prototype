using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ResearchDescriptionlUI : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStatsSO;

    [SerializeField] private TextMeshProUGUI researchNameText;
    [SerializeField] private TextMeshProUGUI researchDescriptionText;
    [SerializeField] private TextMeshProUGUI researchLevelText;
    [SerializeField] private TextMeshProUGUI researchCostText;

    [SerializeField] private Slider progressSlider;
    [SerializeField] private Button setActiveButton;
    [SerializeField] private AudioClip SelectSound;
    private StatType statType;

    private void Start()
    {
        setActiveButton.onClick.AddListener(SetActive);
        Init(playerStatsSO.ActiveResearch);
    }

    public void Init(StatType type)
    {
        statType = type;
        StatSO stat = playerStatsSO.GetStat(type);
        researchNameText.text = stat.Name;
        Refresh();
    }
    public void Refresh()
    {
        StatSO stat = playerStatsSO.GetStat(statType);
        researchDescriptionText.text = !stat.IsMaxLevel() ? stat.GetDescription() : "Completed";
        researchLevelText.text = "Lvl: " + stat.Level.ToString();
        progressSlider.value = stat.Level / (float)stat.MaxLevel;
        researchCostText.text = !stat.IsMaxLevel() ? stat.GetCost().ToString() : "";        
    }
    private void SetActive()
    {
        playerStatsSO.SetActiveResearch(statType);
    }

}
