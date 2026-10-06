using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("RedLightGreenLight");
        }
    }
}
