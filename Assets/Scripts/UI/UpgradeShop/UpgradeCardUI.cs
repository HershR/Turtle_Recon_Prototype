using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{

    [SerializeField] private string upgradeName;
    [SerializeField, TextArea] private string upgradeDescription;
    [SerializeField] private int upgradeCost;


    private Button cardButton;
    private UpgradeDescriptionUI upgradeDescriptionUI;
    private void Start()
    {
        upgradeDescriptionUI = FindObjectOfType<UpgradeDescriptionUI>();
        if(upgradeDescriptionUI == null)
        {
            Debug.LogError("UpgradeCardUI Error: Unable to find UpgradeDesciptionUI panel");
        }
        cardButton = GetComponent<Button>();
        cardButton.onClick.AddListener(OnSelect);
    }

    private void OnSelect()
    {
        upgradeDescriptionUI.Init(upgradeName, upgradeDescription, upgradeCost);
    }

}
