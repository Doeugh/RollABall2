using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransporter : MonoBehaviour
{
    public static int currentGameIndex = 0;

    public static string[] gameScenes =
    {
        "RedLightGreenLight",
        "Dalgona",
        "TugOfWar",
        "JumpRope",
        "Mingle",
        "SquidGame"
    };

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextGame();
        }
    }

    public void LoadNextGame()
    {
        if (currentGameIndex < gameScenes.Length)
        {
            SceneManager.LoadScene(gameScenes[currentGameIndex]);
        }
        else
        {
            Debug.Log("All games completed!");
        }
    }

    public static void AdvanceToNextGame()
    {
        currentGameIndex++;
    }

    public static void SetStartingLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= gameScenes.Length)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }

        currentGameIndex = levelIndex;
    }
}