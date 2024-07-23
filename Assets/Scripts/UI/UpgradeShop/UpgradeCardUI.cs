using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private StatType type;
    [SerializeField] private AudioClip SelectUpgradeSound;
    private UpgradeDescriptionUI upgradeDescriptionUI;

    private void Awake()
    {
        upgradeDescriptionUI = FindObjectOfType<UpgradeDescriptionUI>();
        GetComponent<Button>().onClick.AddListener(OnSelect);
        Debug.Log($"Set UpgradeSescriptionUI {upgradeDescriptionUI != false}");
    }

    public void OnSelect()
    {
        SoundManager.instance.PlaySoundClip(SelectUpgradeSound, transform, 1f);
        upgradeDescriptionUI.Init(type);
    }
}
