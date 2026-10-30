using UnityEngine;

public class LevelWaypointTeleporter : MonoBehaviour
{
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
            }
            else
            {
                Debug.LogError("Waypoint is missing at index: " + index);
            }
        }
        else
        {
            Debug.LogError("Invalid waypoint index: " + index);
        }
    }
}
