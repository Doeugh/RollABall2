using System.Collections;
using UnityEngine;

public class BalanceMinigameManagager : MonoBehaviour
{
    public PlayerController mainPlayer;
    public float knockbackForce = 20f;

    public GameObject minigameBall;
    public Transform ballSpawnPoint;
    public Transform balancePlatform;
    public PlayerMinigameController ballController;

    public float minigameDuration = 5f;

    public float rotationInterval = 1.5f;
    public float maxPlatformAngle = 15f;
    public float platformRotationSpeed = 4f;

    public GameObject minigameUI;

    private bool minigameActive = false;

    private Rigidbody mainPlayerRB;

    private float timer;
    private float rotationTimer;

    private float targetAngle;
    private float originalPlatformAngle;

    private bool previousPlayerMovement;

    private Coroutine minigameCoroutine;

    private void Start()
    {
        mainPlayerRB = mainPlayer.GetComponent<Rigidbody>();

        if (balancePlatform != null)
        {
            originalPlatformAngle = balancePlatform.localEulerAngles.y;
        }

        if (minigameBall != null)
        {
            minigameBall.SetActive(false);
        }

        if (minigameUI != null)
        {
            minigameUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!minigameActive)
        {
            return;
        }

        RotatePlatform();
    }

    public void StartMinigame()
    {
        if (minigameActive)
        {
            return;
        }

        minigameCoroutine = StartCoroutine(MinigameRoutine());
    }

    private IEnumerator MinigameRoutine()
    {
        minigameActive = true;

        if (minigameUI != null)
        {
            minigameUI.SetActive(true);
        }

        previousPlayerMovement = mainPlayer.canMove;
        mainPlayer.canMove = false;

        mainPlayerRB.linearVelocity = Vector3.zero;
        mainPlayerRB.angularVelocity = Vector3.zero;

        balancePlatform.localRotation = Quaternion.Euler(0f, 0f, originalPlatformAngle);

        targetAngle = originalPlatformAngle;

        minigameBall.SetActive(true);

        Rigidbody ballRB = minigameBall.GetComponent<Rigidbody>();

        ballRB.position = ballSpawnPoint.position;
        ballRB.rotation = ballSpawnPoint.rotation;

        ballRB.linearVelocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;

        ballController.canMove = true;

        timer = minigameDuration;
        rotationTimer = 0f;

        while (timer > 0f && minigameActive)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (minigameActive)
        {
            EndMinigame(false);
        }
    }

    private void RotatePlatform()
    {
        rotationTimer += Time.deltaTime;

        if (rotationTimer >= rotationInterval)
        {
            rotationTimer = 0f;

            targetAngle = originalPlatformAngle + Random.Range(-maxPlatformAngle, maxPlatformAngle);
        }

        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

        balancePlatform.localRotation = Quaternion.Slerp(balancePlatform.localRotation, targetRotation, platformRotationSpeed * Time.deltaTime);
    }

    public void PlayerFellOff()
    {
        if (!minigameActive)
        {
            return;
        }

        EndMinigame(true);
    }

    private void EndMinigame(bool failed)
    {
        minigameActive = false;

        ballController.canMove = false;

        Rigidbody ballRB = minigameBall.GetComponent<Rigidbody>();

        ballRB.linearVelocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;

        minigameBall.SetActive(false);

        balancePlatform.localRotation = Quaternion.Euler(0f, 0f, originalPlatformAngle);

        mainPlayer.canMove = previousPlayerMovement;

        if (failed)
        {
            ApplyKnockback();
        }

        if (minigameCoroutine != null)
        {
            StopCoroutine(minigameCoroutine);
            minigameCoroutine = null;
        }

        if (minigameUI != null)
        {
            minigameUI.SetActive(false);
        }
    }

    private void ApplyKnockback()
    {
        float direction = Random.value < 0.5f ? -1f : 1f;

        Vector3 force = mainPlayer.transform.right * direction * knockbackForce;

        mainPlayerRB.AddForce(force, ForceMode.Impulse);
    }
}