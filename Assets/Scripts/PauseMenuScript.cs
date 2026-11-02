using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static PauseMenuScript Instance;

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

    public static void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}