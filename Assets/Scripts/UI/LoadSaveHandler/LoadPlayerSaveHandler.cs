using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerSaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerSaveButtonPrefab;
    [SerializeField] private Transform playerSaveTransform;
    private string[] saves;
    private string selectedSaveName;
    private List<GameObject> saveButtons = new();

    private void OnEnable()
    {
        Init();
    }


    private void OnDisable()
    {
        ResetList();
    }
    private void Init()
    {
        saves = DataPersistenceManager.Instance.GetSaveFiles();
        for (int i = 0; i < saves.Length; i++)
        {
            
            string saveName = saves[0];
            GameObject playerSaveGO = Instantiate(playerSaveButtonPrefab, playerSaveTransform);
            string name = saveName.Split('.')[0];
            var button = playerSaveGO.GetComponent<Button>();
            if (button != null) 
            {
                button.onClick.AddListener(delegate { SetSave(saveName); });
            }
            var text = playerSaveGO.GetComponentInChildren<TextMeshProUGUI>();
            text.text = name;
            saveButtons.Add(playerSaveGO);
        }

    }
    private void ResetList()
    {
        foreach (var button in saveButtons)
        {
            Destroy(button);
        }
        saveButtons.Clear();
    }
    public void SetSave(string save)
    {
        selectedSaveName = save;
    }
    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame(selectedSaveName);
        SceneTransitionManager.instance.LoadGame();
    }


}
