using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] PlayerStatsSO playerStats;
    public GameObject healthBar;
    int numHearts;
    int healthLevel;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite halfHeart;

    void UpdateStretch(float value) {
        RectTransform rt = healthBar.GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(rt.offsetMax.x + value, rt.offsetMax.y);
    }

    void Start() {
        numHearts = player.health;
        healthLevel = playerStats.GetStat(StatType.Health).Level;
        switch(healthLevel) {
            case 1:
                UpdateStretch(75);
                break;
            case 2:
                UpdateStretch(150);
                break;
            case 3:
                UpdateStretch(235);
                break;
            default:
                break;
        }
    }

    void Update() {
        for (int i = 0; i < hearts.Length; i++) {
            // Health exceeds available hearts
            if (player.health > numHearts) {
                numHearts = player.maxHealth;
            }

            // Set heart sprites based on player health
            if (i < player.health) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            // Enable or disable heart sprites based on player health
            if (i < numHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }   
    }

}
