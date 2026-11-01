using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DalgonaController : MonoBehaviour
{
    [SerializeField] private Collider traceChecker;
    [SerializeField] private LayerMask traceLayer;

    [SerializeField] private Transform waypoint;
    [SerializeField] private float checkInterval = 0.1f;

    [SerializeField] private int maxAttempts = 3;
    private int attempts;

    private int requiredLineCount = 0;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text attemptsText;

    private int lineCount = 0;

    private HashSet<GameObject> registeredLines = new HashSet<GameObject>();

    private bool isOnLine = false;
    private bool isRecovering = false;
    private bool isComplete = false;

    private float checkTimer = 0f;

    private void Start()
    {
        attempts = maxAttempts;

        CountTotalLines();

        UpdateProgressUI();
        UpdateAttemptsUI();
    }

    private void Update()
    {
        if (isComplete || isRecovering)
            return;

        checkTimer += Time.deltaTime;

        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CheckForLine();
        }
    }

    private void CheckForLine()
    {
        bool touchingLine = Physics.CheckBox(
            traceChecker.bounds.center,
            traceChecker.bounds.extents,
            traceChecker.transform.rotation,
            traceLayer
        );

        if (touchingLine)
        {
            isOnLine = true;
        }
        else if (isOnLine)
        {
            isOnLine = false;
            LoseAttempt();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isComplete)
            return;

        if (((1 << other.gameObject.layer) & traceLayer) == 0)
            return;

        RegisterLine(other.gameObject);
    }

    private void RegisterLine(GameObject line)
    {
        if (registeredLines.Contains(line))
            return;

        registeredLines.Add(line);

        lineCount++;

        Renderer lineRenderer = line.GetComponent<Renderer>();

        if (lineRenderer != null)
        {
            lineRenderer.material.EnableKeyword("_EMISSION");
            lineRenderer.material.SetColor("_EmissionColor", Color.green * 2f);
        }

        UpdateProgressUI();

        if (lineCount >= requiredLineCount)
        {
            CompleteTracing();
        }
    }

    private void CountTotalLines()
    {
        GameObject[] lines = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        requiredLineCount = 0;

        foreach (GameObject line in lines)
        {
            if (((1 << line.layer) & traceLayer) != 0)
            {
                requiredLineCount++;
            }
        }

        Debug.Log("Total tracing lines found: " + requiredLineCount);
    }

    private void LoseAttempt()
    {
        if (isRecovering || isComplete)
            return;

        attempts--;

        UpdateAttemptsUI();

        if (attempts <= 0)
        {
            FailTracing();
            return;
        }

        RecoverPlayer();
    }

    private void RecoverPlayer()
    {
        isRecovering = true;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = waypoint.position;
        }
        else
        {
            transform.position = waypoint.position;
        }

        isOnLine = false;

        isRecovering = false;
    }

    private void UpdateProgressUI()
    {
        float percentage = 0f;

        if (requiredLineCount > 0)
        {
            percentage = (float)lineCount / requiredLineCount * 100f;
        }

        if (progressText != null)
        {
            progressText.text = $"Progress: {percentage:F0}%";
        }
    }

    private void UpdateAttemptsUI()
    {
        if (attemptsText != null)
        {
            attemptsText.text = $"Attempts: {attempts}";
        }
    }

    private void CompleteTracing()
    {
        if (isComplete)
        {
            return;
        }

        isComplete = true;

        LevelTransporter.AdvanceToNextGame();
        SceneManager.LoadScene("Lobby");
    }

    private void FailTracing()
    {
        isComplete = true;
        SceneManager.LoadScene("YouDiedScreen");
    }
}
