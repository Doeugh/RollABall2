using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject SettingsPanel;

    public AudioMixer audioMixer;

    public void PlayGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OpenSettings()
    {
        Buttons.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
        Buttons.SetActive(true);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
