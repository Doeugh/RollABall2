using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TugGameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject tugUI;
    public GameObject tugObject;
    public Camera tugCamera;

    public Transform rope;
    public Transform[] waypoints;
    public float ropeMoveSpeed = 2f;

    public float TugScore = 0f;
    public float winScore = 5f;
    public float loseScore = -5f;

    public float scoreDrainRate = 0.1f;

    public float checkpointReward = 1f;

    private bool gameStarted = false;
    private bool gameEnded = false;

    private void Start()
    {
        player.SetActive(true);
        tugUI.SetActive(false);
        tugObject.SetActive(false);
        tugCamera.gameObject.SetActive(false);

        TugScore = 0f;

        UpdateRopePosition();
    }

    private void Update()
    {
        if (!gameStarted && !gameEnded && !TOWIntroManager.IsIntroPlaying)
        {
            StartTugGame();
        }

        if (!gameStarted || gameEnded)
        {
            return;
        }

        UpdateRopePosition();
    }

    private void StartTugGame()
    {
        if (gameStarted || gameEnded)
        {
            return;
        }

        gameStarted = true;

        player.SetActive(false);
        tugUI.SetActive(true);
        tugObject.SetActive(true);
        tugCamera.gameObject.SetActive(true);

        TugScore = 0f;

        Debug.Log("Tug Game Started!");

        StartCoroutine(TugScoreDrain());
    }

    private IEnumerator TugScoreDrain()
    {
        while (!gameEnded)
        {
            TugScore -= scoreDrainRate * Time.deltaTime;

            UpdateRopePosition();
            CheckGameOver();

            yield return null;
        }
    }

    public void AddTugScore(float amount = 1f)
    {
        if (!gameStarted || gameEnded)
        {
            return;
        }

        TugScore += amount;

        Debug.Log("Tug Score: " + TugScore.ToString("F2"));

        CheckGameOver();
    }

    private void UpdateRopePosition()
    {
        if (rope == null || waypoints == null || waypoints.Length < 2)
        {
            return;
        }

        float waypointPosition = TugScore - loseScore;

        waypointPosition = Mathf.Clamp(waypointPosition, 0f, waypoints.Length - 1);

        int lowerIndex = Mathf.FloorToInt(waypointPosition);
        int upperIndex = Mathf.Min(lowerIndex + 1, waypoints.Length - 1);

        float interpolation = waypointPosition - lowerIndex;

        if (waypoints[lowerIndex] == null || waypoints[upperIndex] == null)
        {
            return;
        }

        Vector3 targetPosition = Vector3.Lerp(
            waypoints[lowerIndex].position,
            waypoints[upperIndex].position,
            interpolation
        );

        Quaternion targetRotation = Quaternion.Slerp(
            waypoints[lowerIndex].rotation,
            waypoints[upperIndex].rotation,
            interpolation
        );

        rope.position = Vector3.MoveTowards(
            rope.position,
            targetPosition,
            ropeMoveSpeed * Time.deltaTime
        );

        rope.rotation = Quaternion.RotateTowards(
            rope.rotation,
            targetRotation,
            ropeMoveSpeed * 90f * Time.deltaTime
        );
    }

    private void CheckGameOver()
    {
        if (TugScore <= loseScore)
        {
            gameEnded = true;
            SceneManager.LoadScene("YouDiedScreen");
        }
        else if (TugScore >= winScore)
        {
            gameEnded = true;

            LevelTransporter.AdvanceToNextGame();
            SceneManager.LoadScene("Lobby");
        }
    }
}
