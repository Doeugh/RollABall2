using UnityEngine;
using System.Collections;

public class RLGLIntro : MonoBehaviour
{
    public static bool IsIntroPlaying = true;

    public GameObject IntroCam1;
    public GameObject IntroCam2;

    public RLGLCeiling Door1;
    public RLGLCeiling Door2;

    public AudioSource introAudio;
    public GameObject invisibleWall;
    public MonoBehaviour redLightGreenLightScript;
    public RLGLTimer rlglTimer;

    public PlayerController player;

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

        IntroCam1.SetActive(false);
        IntroCam2.SetActive(true);

        Door1.StartMovement();
        Door2.StartMovement();

        yield return new WaitForSeconds(9f);

        IntroCam2.SetActive(false);

        introAudio.Play();
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