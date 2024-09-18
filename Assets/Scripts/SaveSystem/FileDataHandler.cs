using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    public void UpdateDataFileName(string dataFileName)
    {
        this.dataFileName = dataFileName;
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToLoad = "";
                JsonSerializer serializer = new JsonSerializer();
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                {
                    Debug.LogError($"Error occured when tryig to load data from file: {fullPath} \n{e}");
                }
            }
        }
        return loadedData;
    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(data);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            {
                Debug.LogError($"Error occured when tryig to save data to file: {fullPath} \n{e}");
            }
        }
    }
    public string[] GetFiles()
    {
        try
        {
            return Directory.GetFiles(dataDirPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to get files from directory: {dataDirPath} \n {e}");
            return null;
        }
    }
}
