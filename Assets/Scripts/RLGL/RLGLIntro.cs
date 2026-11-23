using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using System.Collections;

public class RLGLIntro : MonoBehaviour
{
    public static bool IsIntroPlaying = true;

    public GameObject IntroCam1;
    public GameObject IntroCam2;

    public RLGLCeiling Door1;
    public RLGLCeiling Door2;

    public PlayableDirector doorTimeline;
    public AudioSource doorSound;

    public AudioSource introAudio;

    public GameObject invisibleWall;
    public MonoBehaviour redLightGreenLightScript;
    public RLGLTimer rlglTimer;

    public PlayerController player;
    public Transform startingPoint;

    public InputActionReference skipAction;

    public GameObject skipPrompt;

    private Coroutine introCoroutine;
    private bool introFinished = false;

    private void OnEnable()
    {
        if (skipAction != null)
        {
            skipAction.action.performed += OnSkipPressed;
            skipAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (skipAction != null)
        {
            skipAction.action.performed -= OnSkipPressed;
        }
    }

    private void Start()
    {
        IsIntroPlaying = true;

        rlglTimer.isRunning = false;
        redLightGreenLightScript.enabled = false;
        invisibleWall.SetActive(true);

        IntroCam1.SetActive(false);
        IntroCam2.SetActive(false);

        if (skipPrompt != null)
        {
            skipPrompt.SetActive(true);
        }

        introCoroutine = StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        IntroCam1.SetActive(true);
        yield return new WaitForSeconds(2f);

        doorTimeline.Play();
        yield return new WaitForSeconds(3f);

        IntroCam1.SetActive(false);
        IntroCam2.SetActive(true);

        yield return new WaitForSeconds(1f);

        Door1.StartMovement();
        Door2.StartMovement();

        if (doorSound != null)
        {
            doorSound.Play();
        }

        yield return new WaitForSeconds(9f);

        IntroCam2.SetActive(false);

        if (introAudio != null)
        {
            introAudio.Play();
        }

        MovePlayerToStartingPoint();

        player.canMove = true;

        if (introAudio != null && introAudio.clip != null)
        {
            yield return new WaitForSeconds(introAudio.clip.length);
        }

        EndIntro();
    }

    private void OnSkipPressed(InputAction.CallbackContext context)
    {
        if (introFinished)
        {
            return;
        }

        EndIntro();
    }

    private void MovePlayerToStartingPoint()
    {
        if (player == null || startingPoint == null)
        {
            return;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = startingPoint.position;
            rb.rotation = startingPoint.rotation;
        }

        player.transform.SetPositionAndRotation(startingPoint.position, startingPoint.rotation);

        player.canMove = true;
    }

    private void EndIntro()
    {
        if (introFinished)
        {
            return;
        }

        introFinished = true;

        if (introCoroutine != null)
        {
            StopCoroutine(introCoroutine);
            introCoroutine = null;
        }

        if (skipPrompt != null)
        {
            skipPrompt.SetActive(false);
        }

        if (introAudio != null)
        {
            introAudio.Stop();
        }

        if (doorSound != null)
        {
            doorSound.Stop();
        }

        if (doorTimeline != null)
        {
            doorTimeline.Stop();
        }

        if (IntroCam1 != null)
        {
            IntroCam1.SetActive(false);
        }

        if (IntroCam2 != null)
        {
            IntroCam2.SetActive(false);
        }

        if (Door1 != null)
        {
            Door1.MoveToEndPoint();
        }

        if (Door2 != null)
        {
            Door2.MoveToEndPoint();
        }

        MovePlayerToStartingPoint();

        IsIntroPlaying = false;

        rlglTimer.isRunning = true;
        redLightGreenLightScript.enabled = true;
        invisibleWall.SetActive(false);
    }
}
