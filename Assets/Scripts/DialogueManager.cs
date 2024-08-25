using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;

    public void ShowDialogue(string message)
    {
        dialogueBox.SetActive(true);  // Show dialogue box
        dialogueText.text = message;  // Set the text
    }

    private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogueText.text = "";  // Clear text
        dialogueBox.SetActive(false);  // Hide dialogue box
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { 
            dialogueBox.SetActive(false);
        }
    }
}
