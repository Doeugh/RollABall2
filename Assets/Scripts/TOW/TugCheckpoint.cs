using UnityEngine;

public class TugCheckpoint : MonoBehaviour
{
    public bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        isActivated = true;

        Debug.Log(gameObject.name + " activated!");
    }

    public void ResetCheckpoint()
    {
        isActivated = false;

        Debug.Log(gameObject.name + " reset!");
    }
}
