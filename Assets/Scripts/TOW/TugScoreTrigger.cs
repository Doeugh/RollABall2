using UnityEngine;

public class TugScoreTrigger : MonoBehaviour
{
    public int TugScore = 0;

    public TugCheckpoint[] checkpoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckAllCheckpoints();
        }
    }

    private void CheckAllCheckpoints()
    {
        foreach (TugCheckpoint checkpoint in checkpoints)
        {
            if (!checkpoint.isActivated)
            {
                Debug.Log("Not all checkpoints have been activated");
                return;
            }
        }

        TugScore++;

        Debug.Log("Tug Score: " + TugScore);

        foreach (TugCheckpoint checkpoint in checkpoints)
        {
            checkpoint.ResetCheckpoint();
        }
    }
}
