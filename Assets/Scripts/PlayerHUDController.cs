using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private GameObject healthUI;
    [SerializeField] private GameObject tokenUI;
    [SerializeField] private GameObject progressUI;

    public void ToggleHealthUI(bool isActive)
    {
        healthUI.SetActive(isActive);
    }
    public void ToggleTokenUI(bool isActive)
    {
        tokenUI.SetActive(isActive);
    }
    public void ToggleProgressUI(bool isActive)
    {
        progressUI.SetActive(isActive);
    }
}
