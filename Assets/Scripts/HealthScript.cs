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

    void UpdateStretch() {
        RectTransform rt = healthBar.GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(rt.offsetMax.x + 75, rt.offsetMax.y);
    }

    void Start() {
        numHearts = player.health;
        healthLevel = playerStats.GetStat(StatType.Health).Level;
        if (healthLevel > 0) {
            for (int i = 0; i < healthLevel; i++) {
                UpdateStretch();
            }
        }
    }

    void Update() {
        for (int i = 0; i < hearts.Length; i++) {
            if (player.health > numHearts) {
                numHearts = player.maxHealth;
            }
            if (i < player.health) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }   
    }

}
