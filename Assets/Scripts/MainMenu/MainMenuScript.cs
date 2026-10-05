using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public MainMenuCamera cameraController;
    public MainMenuUIManager uiManager;

    public void PlayGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OpenSettings()
    {
        uiManager.ShowSettings();
    }

    public void CloseSettings()
    {
        uiManager.ShowMainButtons();
    }

    public void OpenCredits()
    {
        uiManager.ShowCredits();
    }

    public void CloseCredits()
    {
        uiManager.ShowMainButtons();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenMainStory()
    {
        cameraController.GoToMainStory();
    }

    public void OpenRLGL()
    {
        cameraController.GoToRLGL();
    }

    public void OpenDalgona()
    {
        cameraController.GoToDalgona();
    }

    public void OpenTugOfWar()
    {
        cameraController.GoToTugOfWar();
    }

    public void OpenJumpRope()
    {
        cameraController.GoToJumpRope();
    }

    public void OpenMingle()
    {
        cameraController.GoToMingle();
    }

    public void OpenSquidGame()
    {
        cameraController.GoToSquidGame();
    }

    public void OpenMainMenu()
    {
        cameraController.GoToStartingPoint();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}