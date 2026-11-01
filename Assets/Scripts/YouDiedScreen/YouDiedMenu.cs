
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections;

public class YouDiedMenu : MonoBehaviour
{
    public GameObject UiCanvas;

    public PlayableDirector timeline;

    public void GoToMainMenu()
    {
        StartCoroutine(IncineratorCutscene());
    }

    private IEnumerator IncineratorCutscene()
    {
        if (UiCanvas != null)
        {
            UiCanvas.SetActive(false);
        }

        if (timeline != null)
        {
            timeline.Play();
        }

        yield return new WaitForSeconds(6f);

        SceneManager.LoadScene("MainMenu");
    }
}
