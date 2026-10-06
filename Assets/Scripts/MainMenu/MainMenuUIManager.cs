using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    public GameObject settingsPanel;
    public GameObject creditsPanel;

    public GameObject mainStoryPanel;
    public GameObject RLGLPanel;
    public GameObject dalgonaPanel;
    public GameObject tugOfWarPanel;
    public GameObject jumpRopePanel;
    public GameObject minglePanel;
    public GameObject squidGamePanel;

    private void Start()
    {
        ShowMainButtons();
    }

    public void ShowMainButtons()
    {
        HideAllUI();

        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(true);
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

    public void ShowRLGLUI()
    {
        HideAllUI();

        RLGLPanel.SetActive(true);
    }

    public void ShowDalgonaUI()
    {
        HideAllUI();

        dalgonaPanel.SetActive(true);
    }

    public void ShowTugOfWarUI()
    {
        HideAllUI();

        tugOfWarPanel.SetActive(true);
    }

    public void ShowJumpRopeUI()
    {
        HideAllUI();

        jumpRopePanel.SetActive(true);
    }

    public void ShowMingleUI()
    {
        HideAllUI();

        minglePanel.SetActive(true);
    }

    public void ShowSquidGameUI()
    {
        HideAllUI();

        squidGamePanel.SetActive(true);
    }

    public void HideAllUI()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainStoryPanel.SetActive(false);
        RLGLPanel.SetActive(false);
        dalgonaPanel.SetActive(false);
        tugOfWarPanel.SetActive(false);
        jumpRopePanel.SetActive(false);
        minglePanel.SetActive(false);
        squidGamePanel.SetActive(false);
    }
}