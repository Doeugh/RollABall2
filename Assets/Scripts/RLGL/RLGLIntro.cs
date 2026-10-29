using UnityEngine;
using UnityEngine.Playables;
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

    void Start()
    {
        IsIntroPlaying = true;

        rlglTimer.isRunning = false;
        redLightGreenLightScript.enabled = false;
        invisibleWall.SetActive(true);
        IntroCam1.SetActive(false);
        IntroCam2.SetActive(false);

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
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
        doorSound.Play();

        yield return new WaitForSeconds(9f);

        IntroCam2.SetActive(false);

        introAudio.Play();

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = startingPoint.position;
            rb.rotation = startingPoint.rotation;
        }
        player.transform.SetPositionAndRotation(startingPoint.position, startingPoint.rotation);

        player.canMove = true;

        yield return new WaitForSeconds(introAudio.clip.length);

        EndIntro();
    }

    void EndIntro()
    {
        IsIntroPlaying = false;

        rlglTimer.isRunning = true;
        redLightGreenLightScript.enabled = true;
        invisibleWall.SetActive(false);
    }
}