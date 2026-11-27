using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class TOWIntroManager : MonoBehaviour
{
    public static bool IsIntroPlaying = true;

    public PlayableDirector introTimeline;

    public GameObject introCamera;

    public GameObject skipPrompt;

    public GameObject musicManager;

    private bool introFinished = false;

    private void Start()
    {
        IsIntroPlaying = true;

        if (skipPrompt != null)
        {
            skipPrompt.SetActive(true);
        }

        if (musicManager != null)
        {
            musicManager.SetActive(false);
        }

        if (introTimeline != null)
        {
            introTimeline.stopped += OnTimelineFinished;
            introTimeline.Play();
        }
        else
        {
            EndIntro();
        }
    }

    private void Update()
    {
        if (introFinished)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SkipIntro();
        }
    }

    private void SkipIntro()
    {
        if (introTimeline != null)
        {
            introTimeline.time = introTimeline.duration;

            introTimeline.Evaluate();
        }

        EndIntro();
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        EndIntro();
    }

    private void EndIntro()
    {
        if (introFinished)
        {
            return;
        }

        introFinished = true;

        if (introTimeline != null)
        {
            introTimeline.stopped -= OnTimelineFinished;

            if (introTimeline.state == PlayState.Playing)
            {
                introTimeline.Stop();
            }
        }

        if (skipPrompt != null)
        {
            skipPrompt.SetActive(false);
        }

        if (introCamera != null)
        {
            introCamera.SetActive(false);
        }

        if (musicManager != null)
        {
            musicManager.SetActive(true);
        }

        IsIntroPlaying = false;
    }

    private void OnDestroy()
    {
        if (introTimeline != null)
        {
            introTimeline.stopped -= OnTimelineFinished;
        }
    }
}
