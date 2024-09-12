using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    
    // Start is called before the first frame update
    // Starts the dialogue
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    // Checks if the user clicked mouse
    // If they did, show all the characters for that line to skip the text slowly revealing step
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    // Starts typing out the dialogue one line at a time
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    // Types out each character at a set speed
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // Goes to the next text line
    // If no more lines, go to tutorial?
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } 
        else
        {
            // gameObject.SetActive(false);
            // I set the gameobject to not active for now when dialogue ends
            // But it needs to reroute to the tutorial when the introduction is done
            SceneTransitionManager.instance.LoadTutorial();
        }
    }
}
