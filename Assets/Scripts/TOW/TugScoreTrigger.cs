using UnityEngine;

public class TugScoreTrigger : MonoBehaviour
{
    public TugGameManager tugGameManager;

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

        tugGameManager.AddTugScore(1);

        foreach (TugCheckpoint checkpoint in checkpoints)
        {
            checkpoint.ResetCheckpoint();
        }
    }
}
