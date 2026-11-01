using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections;

public class YouDiedMenu : MonoBehaviour
{
    public GameObject UiCanvas;
    public PlayableDirector timeline;

    public float fadeDuration = 0.4f;
    public float cutsceneLength = 6f;

    private bool isRunning;

    public void GoToMainMenu()
    {
        if (isRunning)
        {
            return;
        }

        isRunning = true;

        StartCoroutine(IncineratorCutscene());
    }

    private IEnumerator IncineratorCutscene()
    {
        if (UiCanvas != null)
        {
            CanvasGroup group = UiCanvas.GetComponent<CanvasGroup>();
            if (group == null) group = UiCanvas.AddComponent<CanvasGroup>();

            group.interactable = false;
            group.blocksRaycasts = false;

            float startAlpha = group.alpha;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
                yield return null;
            }

            group.alpha = 0f;
            UiCanvas.SetActive(false);
        }

        if (timeline != null)
        {
            timeline.Play();
        }

        AsyncOperation load = SceneManager.LoadSceneAsync("MainMenu");
        load.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(cutsceneLength);

        load.allowSceneActivation = true;
    }

    public void RetryLevel()
    {
        LevelTransporter.ReloadCurrentGame();
    }
}