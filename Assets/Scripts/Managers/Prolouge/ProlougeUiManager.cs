
using UnityEngine;
using TMPro;
using System.Collections;

public class PrologueUIManager : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI messageText;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    public void ShowInteractionPrompt(bool show)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(show);
        }
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        if (messageText == null)
        {
            yield break;
        }

        messageText.gameObject.SetActive(true);
        messageText.text = message;

        yield return new WaitForSeconds(3f);

        messageText.gameObject.SetActive(false);
    }

    public void ShowDialoguePanel()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
    }

    public void HideDialoguePanel()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void SetDialogueText(string text)
    {
        if (dialogueText != null)
        {
            dialogueText.text = text;
        }
    }
}

