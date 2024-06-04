using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private StatType type;
    
    private UpgradeDescriptionUI upgradeDescriptionUI;

    private void Awake()
    {
        upgradeDescriptionUI = FindObjectOfType<UpgradeDescriptionUI>();
        GetComponent<Button>().onClick.AddListener(OnSelect);
        Debug.Log($"Set UpgradeSescriptionUI {upgradeDescriptionUI != false}");
    }

    public void OnSelect()
    {
        upgradeDescriptionUI.Init(type);
    }
}
