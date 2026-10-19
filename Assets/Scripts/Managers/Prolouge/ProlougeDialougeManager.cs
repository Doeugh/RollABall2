
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PrologueDialogueManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();

    [SerializeField] private PrologueUIManager prologueUIManager;
    [SerializeField] private PlayerController playerController;

    public void StartDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0)
        {
            return;
        }

        sentences.Clear();

        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        prologueUIManager.ShowDialoguePanel();
        prologueUIManager.ShowInteractionPrompt(false);

        DisplayNext();
    }

    public void DisplayNext()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        UpdateDialogueText(sentence);
    }

    private void UpdateDialogueText(string text)
    {
        var field = typeof(PrologueUIManager).GetField("dialogueText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field == null)
        {
            Debug.LogError("Could not find dialogueText in PrologueUIManager.");
            return;
        }

        TMP_Text dialogueText = (TMP_Text)field.GetValue(prologueUIManager);

        if (dialogueText != null)
        {
            dialogueText.text = text;
        }
    }

    private void EndDialogue()
    {
        prologueUIManager.HideDialoguePanel();
    }

}
