using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerSaveManager : MonoBehaviour
{
    [SerializeField] private GameObject playerSaveButtonPrefab;
    [SerializeField] private Transform playerSaveTransform;
    [SerializeField] private string selectedSaveFile;
    [SerializeField] private Button startButton;
    private string[] saves;
    private List<GameObject> saveButtonsList = new();

    private void OnEnable()
    {
        Init();
    }
    private void Start()
    {
        saves = DataPersistenceManager.Instance.GetSaveFiles();
        startButton.onClick.AddListener(LoadGame);
    }

    private void OnDisable()
    {
        ResetList();
    }
    private void Init()
    {
        startButton.interactable = false;
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
            saveButtonsList.Add(playerSaveGO);
        }
    }
    private void ResetList()
    {
        foreach (var button in saveButtonsList)
        {
            Destroy(button);
        }
        saveButtonsList.Clear();
    }
    public void SetSave(string save)
    {
        selectedSaveFile = save;
        startButton.interactable = true;
    }
    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame(selectedSaveFile);
        SceneTransitionManager.instance.LoadGame();
    }


}
