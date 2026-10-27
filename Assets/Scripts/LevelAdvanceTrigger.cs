using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAdvanceTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelTransporter.AdvanceToNextGame();
            SceneManager.LoadScene("Lobby");
        }
    }
}
