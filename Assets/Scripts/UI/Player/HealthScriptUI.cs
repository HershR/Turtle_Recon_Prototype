using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class HealthScriptUI : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    
    [SerializeField] private RectTransform healthBar;
    private Vector2 defaultSizeDelta;
    private float rectWidthGrowthAmount = 90f;

    private float numHearts;

    [SerializeField] private Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite halfHeart;

    private void OnEnable()
    {
        player.onDamageTaken.AddListener(UpdateHearts);
    }

    private void OnDisable()
    {
        player.onDamageTaken.RemoveListener(UpdateHearts);
    }

    void Start() {
        defaultSizeDelta = healthBar.sizeDelta;
        Debug.Log("HEARTY HEARTY HEAR: " + numHearts);
        numHearts = player.health;
        float newWidth = defaultSizeDelta.x + numHearts * rectWidthGrowthAmount;
        healthBar.sizeDelta = new Vector2(newWidth, defaultSizeDelta.y);
        UpdateHearts();
    }
    private void UpdateHearts()
    {
        numHearts = player.health;
        for (int i = 0; i < hearts.Length; i++)
        {

            // Set heart sprites based on player health
            if (i < player.health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            // Enable or disable heart sprites based on player health
            if (i < player.maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }    
}
