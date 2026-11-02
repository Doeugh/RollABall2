using UnityEngine;
using UnityEngine.Playables;

public class LevelWaypointTeleporter : MonoBehaviour
{
    public PlayableDirector[] elevatorDoorTimelines = new PlayableDirector[6];

    public Transform[] waypoints = new Transform[6];

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    public void TeleportPlayer(Transform player)
    {
        int index = LevelTransporter.currentGameIndex;

        if (index >= 0 && index < waypoints.Length)
        {
            if (waypoints[index] != null)
            {
                player.position = waypoints[index].position;
                player.rotation = waypoints[index].rotation;

                if (index < elevatorDoorTimelines.Length && elevatorDoorTimelines[index] != null)
                {
                    PlayableDirector director = elevatorDoorTimelines[index];

                    director.Stop();
                    director.time = 0;
                    director.Evaluate();
                    director.Play();
                }
                else
                {
                    Debug.LogWarning("No Timeline assigned for game index: " + index);
                }
            }
        }
    }
}
