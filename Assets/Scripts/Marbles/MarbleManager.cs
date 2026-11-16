
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MarbleManager : MonoBehaviour
{
    public static MarbleManager Instance;

    public int marbles = 10;
    public TMP_Text marbleCounterText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateMarbleUI();
    }

    public void AddMarbles(int amount)
    {
        if (amount <= 0) return;

        marbles += amount;
        UpdateMarbleUI();
    }

    public bool RemoveMarbles(int amount)
    {
        if (amount <= 0 || amount > marbles)
        {
            return false;
        }

        marbles -= amount;
        UpdateMarbleUI();

        return true;
    }

    public bool CheckForGameOver()
    {
        if (marbles <= 0)
        {
            SceneManager.LoadScene("YouDiedScreen");
            return true;
        }

        return false;
    }

    private void UpdateMarbleUI()
    {
        if (marbleCounterText != null)
        {
            marbleCounterText.text = "" + marbles;
        }
    }
}
