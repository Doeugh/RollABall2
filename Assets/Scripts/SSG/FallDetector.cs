using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDetector : MonoBehaviour
{
    public RoundManager roundManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            roundManager.RegisterFall(other.gameObject);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Player"))
        {
           SceneManager.LoadScene("YouDiedScreen");
        }
    }
}
