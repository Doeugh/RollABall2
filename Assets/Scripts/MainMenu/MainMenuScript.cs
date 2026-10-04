using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject SettingsPanel;
    public GameObject CreditsPanel;

    public AudioMixer audioMixer;

    public void Start()
    {
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

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
        Buttons.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        CreditsPanel.SetActive(false);
        Buttons.SetActive(true);
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
