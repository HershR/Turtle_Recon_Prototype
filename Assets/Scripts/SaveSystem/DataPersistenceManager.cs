using System.Collections.Generic;
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
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }
    public void NewGame(string playerName)
    {
        SetFileName(playerName);
        gameData = new GameData();
        gameData.playerName = playerName;
        SaveGame();
        LoadGame();//reset all data
    }
    public void LoadGame()
    {
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
        statsSO.SaveData(gameData);
        Debug.Log(
            $"Saved Player: {statsSO.Name},\n " +
            $"Tokens: {statsSO.Tokens},\n " +
            $"High Score: {statsSO.HighScore}");
        dataHandler.Save(gameData);
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
    public void SetFileName(string playerName)
    {
        fileName = $"{playerName}.save.json";
        dataHandler.SetDataFileName(fileName);
    }
}
