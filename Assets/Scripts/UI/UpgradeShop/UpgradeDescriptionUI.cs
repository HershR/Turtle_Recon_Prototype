using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeDescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    
    [SerializeField] private Slider upgradeLevelSlider;
    [SerializeField] private Button upgradePurchaceButton;

    private void Start()
    {
        upgradePurchaceButton.onClick.AddListener(BuyUpgrade);
    }

    public void Init(
        string upgradeName, 
        string upgradeDescription,
        int upgradeCost=0, 
        int upgradeLevel=0)
    {
        this.upgradeName.text = upgradeName;
        this.upgradeDescription.text =upgradeDescription;
        upgradeLevelSlider.value = upgradeLevel / 5;
        this.upgradeCost.text = upgradeCost.ToString();
        upgradePurchaceButton.interactable = upgradeCost >= 0; 
    }

    private void BuyUpgrade()
    {

    }


}
