using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public void ShowDialogue(string message)
    {
        dialogueText.text = message;
        StartCoroutine(ClearTextAfterDelay(5)); // Clear text after 5 seconds
    }

    private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogueText.text = ""; // Clear text
    }
}
