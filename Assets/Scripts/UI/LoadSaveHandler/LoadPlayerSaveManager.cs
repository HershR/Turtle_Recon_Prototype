using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerSaveManager : MonoBehaviour
{
    [SerializeField] private GameObject playerSaveButtonPrefab;
    [SerializeField] private Transform playerSaveTransform;
    [SerializeField] private string selectedSaveName;
    [SerializeField] private Button startButton;
    private string[] saves;
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
            string saveFileName = saves[i];
            string saveName = saveFileName.Split('.')[0];
            GameObject playerSaveGO = Instantiate(playerSaveButtonPrefab, playerSaveTransform);
            var button = playerSaveGO.GetComponent<Button>();
            if (button != null) 
            {
                button.onClick.AddListener(delegate { SetSave(saveFileName); });
            }
            var text = playerSaveGO.GetComponentInChildren<TextMeshProUGUI>();
            text.text = saveName;
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
        startButton.interactable = true;
    }
    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame(selectedSaveName);
        SceneTransitionManager.instance.LoadGame();
    }


}
