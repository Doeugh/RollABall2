using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public GameObject settingsPanel;
    public GameObject creditsPanel;

    public GameObject mainStoryPanel;

    private void Start()
    {
        ShowMainButtons();
    }

    public void ShowMainButtons()
    {
        HideAllUI();

        mainMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAllUI();

        settingsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        HideAllUI();

        creditsPanel.SetActive(true);
    }

    public void ShowMainStoryUI()
    {
        HideAllUI();

        mainStoryPanel.SetActive(true);
    }

    public void HideAllUI()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainStoryPanel.SetActive(false);
    }
}