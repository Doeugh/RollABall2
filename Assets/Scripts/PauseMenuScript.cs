using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static PauseMenuScript Instance;

    public GameObject controlsMenu;
    public GameObject optionsMenu;

    public AudioMixer audioMixer;

    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    public static void TogglePause()
    {
        Instance.isPaused = !Instance.isPaused;

        Instance.pauseMenuUI.SetActive(Instance.isPaused);

        Time.timeScale = Instance.isPaused ? 0f : 1f;

        Cursor.lockState = Instance.isPaused ? CursorLockMode.None: CursorLockMode.Locked;

        Cursor.visible = Instance.isPaused;
    }

    public static void Resume()
    {
        if (Instance.isPaused)
        {
            TogglePause();
        }
    }

    public void ShowControls()
    {
        HideAll();

        controlsMenu.SetActive(true);
    }

    public void HideControls()
    {
        HideAll();
        pauseMenuUI.SetActive(true);
    }

    public void ShowOptions()
    {
        HideAll();
        optionsMenu.SetActive(true);
    }

    public void HideOptions()
    {
        HideAll();
        pauseMenuUI.SetActive(true);
    }

    public static void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void HideAll()
    {
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        pauseMenuUI.SetActive(false);
    }
}