using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToShopButtonUI : MonoBehaviour
{
    public void LoadUpgradeShop()
    {
        SceneManager.LoadScene("UpgradeScene");
    }
}
