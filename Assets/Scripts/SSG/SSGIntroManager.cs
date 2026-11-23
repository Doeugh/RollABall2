using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SSGIntroManager : MonoBehaviour
{
    public PlayerController player;

    public SSQEnemyMovement[] enemies;

    public AudioSource introAudio;
    public AudioSource backgroundAudio;

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
        if (player != null)
        {
            player.canMove = false;
        }

        foreach (SSQEnemyMovement enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.EnemyCantMove = true;
                enemy.StopMovement();
            }
        }

        if (backgroundAudio != null)
        {
            backgroundAudio.Stop();
        }

        if (skipPrompt != null)
        {
            skipPrompt.SetActive(true);
        }

        introCoroutine = StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        if (introAudio != null && introAudio.clip != null)
        {
            introAudio.Play();

            yield return new WaitWhile(() => introAudio != null && introAudio.isPlaying && !introFinished);
        }

        FinishIntro();
    }

    private void OnSkipPressed(InputAction.CallbackContext context)
    {
        if (introFinished)
        {
            return;
        }

        FinishIntro();
    }

    private void FinishIntro()
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

        if (backgroundAudio != null)
        {
            backgroundAudio.Play();
        }

        if (player != null)
        {
            player.canMove = true;
        }

        foreach (SSQEnemyMovement enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.EnemyCantMove = false;
            }
        }
    }
}
