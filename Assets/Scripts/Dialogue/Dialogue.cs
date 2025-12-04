using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class CollisionDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Reference to the TextMeshProUGUI component for displaying dialogue
    public string[] lines; // Array of dialogue lines
    public float textSpeed; // Speed of text display
    private int index; // Current line index
    public GameObject triggerObj;
    public bool hasTriggered;

    private void Awake()
    {
        hasTriggered = false;
        gameObject.SetActive(false); // start hidden
    }
    public void TriggerDialogue()
    {
        index = 0;
        dialogueText.text = "";
        gameObject.SetActive(true);
        
        StartDialogue();
    }
   
    private void Update()
    {
        hasTriggered = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueText.text == lines[index]) // If the current line is fully displayed
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];// Instantly display the full line
            }
        }
    }


    void StartDialogue()
    {
        hasTriggered = true;
        index = 0;
        StartCoroutine(TypeLine()); // Start typing the first line
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

    }
    void NextLine()
    {
        
        if (index < lines.Length - 1)
        {
            index++; // Move to the next line
            dialogueText.text = ""; // Clear the text
            StartCoroutine(TypeLine()); // Start typing the next line
            
        }
        else
        {
            gameObject.SetActive(false); // Deactivate dialogue box when done
        }
    }
}