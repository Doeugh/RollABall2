using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public GameObject guessingGamePanel;
    public Button leaveGameButton;

    public MarbleGuessingGame1 guessingGame;

    public PlayerController playerController;

    private bool playerNearby;
    private bool gameOpen;
    private bool previousCanMove;

    public int marblesNeededToAdvance = 20;

    private void Start()
    {
        interactionPrompt.SetActive(false);
        guessingGamePanel.SetActive(false);

        leaveGameButton.onClick.AddListener(CloseGame);
    }

    private void Update()
    {
        if (gameOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (playerNearby && Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            OpenGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (!gameOpen)
            {
                interactionPrompt.SetActive(true);
            }
                
            if (playerController == null)
            {
                playerController = other.GetComponentInParent<PlayerController>();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            interactionPrompt.SetActive(false);
        }
    }

    public void OpenGame()
    {
        if (gameOpen)
        {
            return;
        }

        EnemyMarbles enemyMarbles = GetComponent<EnemyMarbles>();

        if (enemyMarbles == null)
        {
            Debug.LogError("EnemyMarbles component missing!");
            return;
        }

        gameOpen = true;

        if (playerController != null)
        {
            previousCanMove = playerController.canMove;
            playerController.canMove = false;
        }

        interactionPrompt.SetActive(false);
        guessingGamePanel.SetActive(true);

        guessingGame.BeginGame(enemyMarbles);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void CloseGame()
    {
        if (!gameOpen)
        {
            return;
        }

        if (MarbleManager.Instance.CheckForGameOver())
        {
            return;
        }

        gameOpen = false;
        guessingGamePanel.SetActive(false);

        if (MarbleManager.Instance.marbles >= 20)
        {
            LevelTransporter.AdvanceToNextGame();
            SceneManager.LoadScene("Lobby");
            return;
        }

        if (playerController != null)
        {
            playerController.canMove = previousCanMove;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerNearby)
            interactionPrompt.SetActive(true);
    }

}
