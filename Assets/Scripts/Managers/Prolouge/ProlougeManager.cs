using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PrologueManager : MonoBehaviour
{
    public PlayerMinigameController playerController;
    public Rigidbody playerRb;

    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public GameObject dialogueBox;

    public CanvasGroup blackScreen;
    public float fadeDuration = 2f;

    public Transform recruiter;
    public Transform recruiterStartPosition;
    public Transform recruiterEndPosition;
    public float recruiterRollSpeed = 2f;

    public GameObject blueSquare;
    public GameObject redSquare;

    public GameObject controlsUI;

    public Camera dialogueCamera;
    public Camera gameplayCamera;
    public GameObject cutsceneCamera;

    public GameObject looseDialouge;

    private bool roundActive = false;
    private bool roundFinished = false;
    public bool firstTry = true;

    private float remainingTime;
    public float timeLimit = 15f;

    public TMP_Text timerText;

    public float endingDelay = 3f;

    public Transform playerStartingPosition;

    public PlayableDirector winTimeline;
    public PlayableDirector loseTimeline;

    private void Start()
    {
        playerController.canMove = false;

        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        blackScreen.alpha = 1f;

        dialogueBox.SetActive(false);
        blueSquare.SetActive(false);
        redSquare.SetActive(false);
        controlsUI.SetActive(false);

        dialogueCamera.gameObject.SetActive(true);
        gameplayCamera.gameObject.SetActive(false);

        StartCoroutine(PrologueSequence());
    }

    private IEnumerator PrologueSequence()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeFromBlack());

        yield return StartCoroutine(RollRecruiter());

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Say("Recruiter", "Hello there"));
        yield return StartCoroutine(Say("Recruiter", "Would you like to play a game with me?"));

        yield return StartCoroutine(Say("You", "Who are you, you a salesman?"));
        yield return StartCoroutine(Say("You", "You guys got a whole sort of tactics."));

        blueSquare.SetActive(true);
        redSquare.SetActive(true);

        yield return StartCoroutine(Say("Recruiter", "Im sure youve played this before"));
        yield return StartCoroutine(Say("Recruiter", "Play a few rounds of this with me, and each time you win ill pay you"));

        yield return StartCoroutine(Say("Recruiter", "All you gotta do is flip my square then i pay you, if i flip your square, you pay me"));

        yield return StartCoroutine(Say("Recruiter", "You can also be the first to play"));

        yield return StartCoroutine(Say("You", "Look ive had a rough day, if you pull a scam on me.. ill uh ill kill you"));

        dialogueBox.SetActive(false);
        yield return new WaitForSeconds(2f);
        dialogueBox.SetActive(true);

        yield return StartCoroutine(Say("You", "..."));
        yield return StartCoroutine(Say("You", "So its my turn"));

        yield return StartCoroutine(Say("Recruiter", "Youll be blue"));

        dialogueBox.SetActive(false);

        dialogueCamera.gameObject.SetActive(false);
        gameplayCamera.gameObject.SetActive(true);

        controlsUI.SetActive(true);

        StartCoroutine(StartRound());
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            blackScreen.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        blackScreen.alpha = 0f;
        blackScreen.gameObject.SetActive(false);
    }

    private IEnumerator RollRecruiter()
    {
        recruiter.position = recruiterStartPosition.position;

        while (Vector3.Distance(recruiter.position, recruiterEndPosition.position) > 0.05f)
        {
            Vector3 direction = (recruiterEndPosition.position - recruiter.position).normalized;

            recruiter.position = Vector3.MoveTowards(recruiter.position, recruiterEndPosition.position, recruiterRollSpeed * Time.deltaTime);

            recruiter.Rotate(Vector3.Cross(Vector3.up, direction) * (360f / (2f * Mathf.PI)) * recruiterRollSpeed * Time.deltaTime, Space.World);

            yield return null;
        }

        recruiter.position = recruiterEndPosition.position;
    }

    private IEnumerator Say(string speaker, string line)
    {
        dialogueBox.SetActive(true);

        speakerText.text = speaker;
        dialogueText.text = line;

        while (!Keyboard.current.enterKey.wasPressedThisFrame)
        {
            yield return null;
        }

        yield return null;
    }

    private IEnumerator StartRound()
    {
        playerRb.position = playerStartingPosition.position;
        playerRb.rotation = playerStartingPosition.rotation;

        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        cutsceneCamera.SetActive(false);
        looseDialouge.SetActive(false);
        controlsUI.SetActive(true);
        playerController.canMove = true;

        yield return new WaitForSeconds(1f);

        remainingTime = timeLimit;
        roundActive = true;

        timerText.gameObject.SetActive(true);

        while (remainingTime > 0 && roundActive)
        {
            remainingTime -= Time.deltaTime;

            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime);

            yield return null;
        }

        if (roundActive)
        {
            if (DajkiTrigger.IsInsideDakji)
            {
                PlayerWon();
            }
            else
            {
                PlayerLost();
            }
        }
    }

    public void PlayerWon()
    {
        if (!roundActive || roundFinished)
        {
            return;
        }

        roundFinished = true;
        roundActive = false;

        controlsUI.SetActive(false);

        if (firstTry)
        {
            StartCoroutine(FirstTryWinSequence());
        }
        else
        {
            StartCoroutine(WinSequence());
        }
    }

    private void PlayerLost()
    {
        if (roundFinished)
        {
            return;
        }

        roundFinished = true;
        roundActive = false;

        controlsUI.SetActive(false);

        StartCoroutine(LoseSequence());
    }

    private IEnumerator FirstTryWinSequence()
    {
        timerText.gameObject.SetActive(false);

        playerController.canMove = false;

        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        winTimeline.Play();

        yield return new WaitUntil(() => winTimeline.state != PlayState.Playing);

        SceneManager.LoadScene("Lobby");
    }

    private IEnumerator WinSequence()
    {
        timerText.gameObject.SetActive(false);

        playerController.canMove = false;

        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        winTimeline.Play();

        yield return new WaitUntil(() => winTimeline.state != PlayState.Playing);

        SceneManager.LoadScene("Lobby");
    }

    private IEnumerator LoseSequence()
    {
        timerText.gameObject.SetActive(false);

        playerController.canMove = false;

        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        loseTimeline.Play();

        yield return new WaitUntil(() => loseTimeline.state != PlayState.Playing);

        roundFinished = false;
        roundActive = false;
        firstTry = false;

        StartCoroutine(StartRound());
    }
}

