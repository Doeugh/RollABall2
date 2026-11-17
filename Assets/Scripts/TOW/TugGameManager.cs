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

    public int TugScore = 0;
    public int winScore = 5;
    public int loseScore = -5;

    private bool gameStarted = false;
    private bool gameEnded = false;

    private Coroutine ropeMovement;

    private void Start()
    {
        player.SetActive(true);
        tugUI.SetActive(false);
        tugObject.SetActive(false);
        tugCamera.gameObject.SetActive(false);

        TugScore = 0;

        UpdateRopePosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameStarted || gameEnded)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            StartTugGame();
        }
    }

    private void StartTugGame()
    {
        gameStarted = true;

        player.SetActive(false);
        tugUI.SetActive(true);
        tugObject.SetActive(true);
        tugCamera.gameObject.SetActive(true);

        TugScore = 0;

        UpdateRopePosition();

        Debug.Log("Tug Game Started!");

        StartCoroutine(DecreaseTugScore());
    }

    private IEnumerator DecreaseTugScore()
    {
        while (!gameEnded)
        {
            float waitTime = Random.Range(4.50f, 7.50f);

            yield return new WaitForSeconds(waitTime);

            if (gameEnded)
            {
                yield break;
            }

            TugScore--;

            Debug.Log("Tug Score: " + TugScore);

            UpdateRopePosition();
            CheckGameOver();
        }
    }

    public void AddTugScore(int amount = 1)
    {
        if (!gameStarted || gameEnded)
        {
            return;
        }

        TugScore += amount;

        Debug.Log("Tug Score: " + TugScore);

        UpdateRopePosition();
        CheckGameOver();
    }

    private void UpdateRopePosition()
    {
        if (rope == null || waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        int waypointIndex = TugScore - loseScore;

        waypointIndex = Mathf.Clamp(waypointIndex, 0, waypoints.Length - 1);

        if (waypoints[waypointIndex] == null)
        {
            return;
        }

        if (ropeMovement != null)
        {
            StopCoroutine(ropeMovement);
        }

        ropeMovement = StartCoroutine(MoveRope(waypoints[waypointIndex]));
    }

    private IEnumerator MoveRope(Transform target)
    {
        while (Vector3.Distance(rope.position, target.position) > 0.01f)
        {
            rope.position = Vector3.MoveTowards(rope.position, target.position, ropeMoveSpeed * Time.deltaTime);

            rope.rotation = Quaternion.RotateTowards(rope.rotation, target.rotation, ropeMoveSpeed * 90f * Time.deltaTime);

            yield return null;
        }

        rope.position = target.position;
        rope.rotation = target.rotation;

        ropeMovement = null;
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
