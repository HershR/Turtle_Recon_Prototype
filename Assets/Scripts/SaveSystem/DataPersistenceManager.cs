using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    private string fileName = ""; //format: <player_name>.save.json
    
    [Header("To save/load")]
    //since we only really need the statsSO we wont use the list
    //in the future, if we have more objects that need to be save, then
    //we need to find all IDataPersistence and store them, when load and saving
    //loop through them
    //[SerializeField] private List<IDataPersistence> dataPersistenceObjs;
    [SerializeField] private PlayerStatsSO statsSO;
    private string saveDataPath;

    private GameData gameData;
    private FileDataHandler dataHandler;
    
    public static DataPersistenceManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        gameData = new GameData();
        saveDataPath = Path.Combine(Application.persistentDataPath, "saves");
        dataHandler = new FileDataHandler(saveDataPath, "temp.save.json");
    }
    public void NewGame(string playerName)
    {
        string saveFileName = NameToSaveFileName(playerName);
        fileName = saveFileName;
        dataHandler = new FileDataHandler(saveDataPath, saveFileName);
        statsSO.ResetAllStats();
        statsSO.SetPlayerName(playerName);
        SaveGame();
    }
    public void LoadGame(string saveFileName)
    {
        fileName = saveFileName;
        dataHandler = new FileDataHandler(saveDataPath, saveFileName);
        gameData = dataHandler.Load();
        if (gameData == null) 
        {
            Debug.LogError("No Data Found");
            return;
        }
        statsSO.LoadData(gameData);
        Debug.Log(
            $"Loaded Player: {statsSO.Name},\n " +
            $"Tokens: {statsSO.Tokens},\n " +
            $"High Score: {statsSO.HighScore}");
    }
    public void SaveGame()
    {
        if(fileName == null) { return; }
        if(dataHandler == null) { return; }
        statsSO.SaveData(gameData);
        Debug.Log(
            $"Saved Player: {statsSO.Name},\n " +
            $"Tokens: {statsSO.Tokens},\n " +
            $"High Score: {statsSO.HighScore}");
        dataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public string[] GetSaveFiles()
    {
        return dataHandler.GetFiles();
    }
    public string[] GetSaveNames()
    {
        string[] fileNames = GetSaveFiles();
        if (fileNames != null)
        {
            for (int i = 0; i < fileNames.Length; i++)
            {
                var nameSplit = fileNames[i].Split(".");
                fileNames[i] = nameSplit[0];
            }
        }
        return fileNames;
    }
    public string NameToSaveFileName(string playerName)
    {
        if (playerName == null) 
        {
            Debug.LogError("Error: NameToSaveFileName() passed in null string as param");
            return null; 
        }
        playerName = playerName.Trim();
        return $"{playerName}.save.json";
    }
}
