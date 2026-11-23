using UnityEngine;

public class TugScoreTrigger : MonoBehaviour
{
    public TugGameManager tugGameManager;

    public TugCheckpoint[] checkpoints;

    public GameObject spinningArrow;

    private bool hasScored = false;

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
                return;
            }
        }

        tugGameManager.AddTugScore(tugGameManager.checkpointReward);

        if (!hasScored)
        {
            spinningArrow.SetActive(false);
            hasScored = true;
        }

        foreach (TugCheckpoint checkpoint in checkpoints)
        {
            checkpoint.ResetCheckpoint();
        }
    }
}
