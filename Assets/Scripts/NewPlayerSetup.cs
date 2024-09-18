using System.Linq;
using TMPro;
using UnityEngine;

public class NewPlayerSetup : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private TextMeshProUGUI alertText;
    public void CreateNewGame()
    {
        if (ValidateName(playerName))
        {
            DataPersistenceManager.Instance.NewGame(playerName);
            SceneTransitionManager.instance.LoadTutorial();
        }
    }
    public void SetName(string newName)
    {
        if (ValidateName(newName))
        {
            playerName = newName;
        }
    }
    public bool ValidateName(string nameToCheck)
    {
        Debug.Log("Validating: " + nameToCheck);
        nameToCheck = nameToCheck.Trim();
        if (nameToCheck == null || nameToCheck.Length == 0)
        {
            alertText.text = "Name can not be empty";
            return false;
        }
        if (!nameToCheck.All(char.IsLetterOrDigit))
        {
            alertText.text = "Name can only contain letters and numbers";
            return false;
        }

        string[] existingPlayers = DataPersistenceManager.Instance.GetSaveFiles();
        if (existingPlayers != null && existingPlayers.Length > 0)
        {
            if (existingPlayers.Contains(nameToCheck)) 
            {
                alertText.text = "Name is already taken"; //or check highscore thing
                return false;
            }
        }
        alertText.text = "";
        return true;
    }
}
