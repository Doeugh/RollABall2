using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RoundSpawnSet
{
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;
}

public class RoundManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public SSGEnemyAI[] enemies;
    public RoundSpawnSet[] roundSpawns;

    [Header("Settings")]
    public float roundDuration = 180f;
    public int totalRounds = 3;

    [Header("UI")]
    public TMP_Text timerText;

    public float accelerationRate = 0.15f;
    public float maxTimerSpeed = 5f;

    public float eliminationAcceleration = 0.5f;

    private int enemiesEliminatedThisRound = 0;

    private float currentTimerSpeed = 1f;

    public bool roundStarted = false;
    public bool RoundComplete = false;
    public bool gameEnded = false;

    private int currentRound = 0;
    private Coroutine timerCoroutine;

    void Start()
    {
        StartCoroutine(InitialSetup());
    }


    IEnumerator InitialSetup()
    {
        yield return null;

        Debug.Log("RoundManager: " + gameObject.name + " | Spawn Sets: " + (roundSpawns == null ? "NULL" : roundSpawns.Length.ToString()) + " | Total Rounds: " + totalRounds);

        if (roundSpawns == null || roundSpawns.Length < totalRounds)
        {
            Debug.LogError("Not enough round spawn sets assigned on: " + gameObject.name);

            yield break;
        }

        TeleportEveryoneToRound();
    }


    public void StartRound()
    {
        if (roundStarted || gameEnded)
        {
            return;
        }

        if (!AreAnyEnemiesAlive())
        {
            Debug.Log("All enemies defeated! Player wins!");

            EndGame(true);
            return;
        }

        roundStarted = true;
        RoundComplete = false;

        currentTimerSpeed = 1f;
        enemiesEliminatedThisRound = 0;

        Debug.Log("Round " + (currentRound + 1) + " started!");

        timerCoroutine = StartCoroutine(RoundTimer());
    }

    public void RegisterFall(GameObject fallenObject)
    {
        if (!roundStarted || gameEnded)
        {
            return;
        }

        if (RoundComplete)
        {
            return;
        }

        Debug.Log(fallenObject.name + " has fallen!");

        SSGEnemyAI ai = fallenObject.GetComponent<SSGEnemyAI>();

        if (ai != null)
        {
            enemiesEliminatedThisRound++;

            Debug.Log("Enemies eliminated this round: " + enemiesEliminatedThisRound);
        }

        RoundComplete = true;

        Debug.Log("Round objective completed.");

        Rigidbody rb = fallenObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        PlayerController pc = fallenObject.GetComponent<PlayerController>();

        if (pc != null)
        {
            pc.canMove = false;
        }

        if (ai != null)
        {
            ai.canMove = false;
        }
    }

    IEnumerator RoundTimer()
    {
        float timeRemaining = roundDuration;

        currentTimerSpeed = 1f;

        while (timeRemaining > 0)
        {
            UpdateTimerUI(timeRemaining);

            if (RoundComplete)
            {
                currentTimerSpeed += accelerationRate * Time.deltaTime;
            }
            else
            {
                currentTimerSpeed = 1f + (enemiesEliminatedThisRound * eliminationAcceleration);
            }

            currentTimerSpeed = Mathf.Min(currentTimerSpeed, maxTimerSpeed);

            timeRemaining -= currentTimerSpeed * Time.deltaTime;

            yield return null;
        }

        UpdateTimerUI(0);

        FinishRound();
    }

    void FinishRound()
    {
        roundStarted = false;

        if (!RoundComplete)
        {
            Debug.Log("Nobody fell! Everyone loses.");

            EndGame(false);
            return;
        }

        Debug.Log("Round completed successfully!");

        currentRound++;

        if (currentRound >= totalRounds)
        {
            EndGame(true);
            return;
        }

        TeleportEveryoneToRound();

        RoundComplete = false;
        roundStarted = false;

        UpdateTimerUI(roundDuration);

        Debug.Log("Ready for round " + (currentRound + 1));
    }

    void TeleportEveryoneToRound()
    {
        RoundSpawnSet spawnSet = roundSpawns[currentRound];

        if (player != null && spawnSet.playerSpawnPoint != null)
        {
            TeleportObject(player.gameObject, spawnSet.playerSpawnPoint);
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                continue;
            }

            if (!enemies[i].gameObject.activeInHierarchy)
            {
                continue;
            }

            if (i >= spawnSet.enemySpawnPoints.Length)
            {
                Debug.LogWarning("Missing enemy spawn point for enemy " + i);
                continue;
            }

            if (spawnSet.enemySpawnPoints[i] == null)
                continue;

            TeleportObject(enemies[i].gameObject, spawnSet.enemySpawnPoints[i]);
        }
    }

    void TeleportObject(GameObject obj, Transform spawn)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = spawn.position;
            rb.rotation = spawn.rotation;
        }
        else
        {
            obj.transform.SetPositionAndRotation(spawn.position, spawn.rotation);
        }
    }

    void EndGame(bool victory)
    {
        gameEnded = true;
        roundStarted = false;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        if (victory)
        {
            SceneManager.LoadScene("WinCutscene");
        }
        else
        {
            SceneManager.LoadScene("YouDiedScreen");
        }
    }

    void UpdateTimerUI(float time)
    {
        if (timerText == null)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        timerText.color = RoundComplete ? Color.green : Color.red;
    }

    bool AreAnyEnemiesAlive()
    {
        foreach (SSGEnemyAI enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            if (enemy.gameObject.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }

    public void RegisterEnemyElimination()
    {
        if (!roundStarted || gameEnded || RoundComplete)
        {
            return;
        }

        enemiesEliminatedThisRound++;

        Debug.Log("Enemies eliminated this round: " + enemiesEliminatedThisRound);
    }
}