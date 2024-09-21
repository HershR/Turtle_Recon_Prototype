using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerSetup : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private string[] existingPlayers;
    [SerializeField] private Button startButton;

    private void Start()
    {
        var existingSaves = DataPersistenceManager.Instance.GetSaveFiles();
        existingPlayers = existingSaves.Select(x => x.Split('.')[0]).ToArray();
        startButton.onClick.AddListener(CreateNewGame);
    }
    public void CreateNewGame()
    {
        if (ValidateName(playerName))
        {
            DataPersistenceManager.Instance.NewGame(playerName);
            //DataPersistenceManager.Instance.LoadGame();
            SceneTransitionManager.instance.LoadIntroduction();
        }
    }
    public void SetName(string newName)
    {
        if (ValidateName(newName))
        {
            playerName = newName;
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
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

        if (existingPlayers != null && existingPlayers.Length > 0)
        {
            if (existingPlayers.Contains(nameToCheck)) 
            {
                alertText.text = "Name is already taken"; //or check highscore thing
                return false;
            }
            // check if name exist on leaderboard
        }
        alertText.text = "";
        return true;
    }
}
