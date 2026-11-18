using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarbleGuessingGame1 : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField betInput;
    public TMP_Text resultText;
    public TMP_Text enemyInventoryText;

    public Button oddButton;
    public Button evenButton;
    public Button playAgainButton;

    private EnemyMarbles currentEnemy;

    private int currentBet;
    private int enemyHandAmount;
    private bool roundFinished;

    private void Start()
    {
        oddButton.onClick.AddListener(() => MakeGuess(true));
        evenButton.onClick.AddListener(() => MakeGuess(false));

        playAgainButton.onClick.AddListener(OnPlayAgain);
        playAgainButton.gameObject.SetActive(false);
    }

    public void BeginGame(EnemyMarbles enemy)
    {
        currentEnemy = enemy;

        if (currentEnemy == null)
        {
            Debug.LogError("No EnemyMarbles component found!");
            return;
        }

        ResetRound();
    }

    public void MakeGuess(bool guessedOdd)
    {
        if (roundFinished || currentEnemy == null)
        {
            return;
        }

        if (!int.TryParse(betInput.text, out currentBet))
        {
            resultText.text = "Enter a valid bet!";
            return;
        }

        if (currentBet <= 0 || currentBet > MarbleManager.Instance.marbles)
        {
            resultText.text = "Invalid bet amount!";
            return;
        }

        enemyHandAmount = currentEnemy.GetHandAmount();
        bool enemyHasOdd = enemyHandAmount % 2 != 0;

        if (guessedOdd == enemyHasOdd)
        {
            MarbleManager.Instance.AddMarbles(currentBet);
            currentEnemy.RemoveMarbles(currentBet);

            resultText.text = "CORRECT! You won " + currentBet + " marbles!";
        }
        else
        {
            MarbleManager.Instance.RemoveMarbles(currentBet);
            currentEnemy.AddMarbles(currentBet);

            resultText.text = "WRONG! You lost " + currentBet + " marbles.";
        }

        roundFinished = true;
        oddButton.interactable = false;
        evenButton.interactable = false;
        playAgainButton.gameObject.SetActive(true);
    }

    private void OnPlayAgain()
    {
        if (MarbleManager.Instance.CheckForGameOver())
        {
            return;
        }

        ResetRound();
    }

    public void ResetRound()
    {
        if (currentEnemy == null)
            return;

        roundFinished = false;
        betInput.text = "";
        resultText.text = "";

        enemyInventoryText.text = "They own: " + currentEnemy.CurrentMarbles + " marbles";

        oddButton.interactable = true;
        evenButton.interactable = true;
        playAgainButton.gameObject.SetActive(false);
    }
}
