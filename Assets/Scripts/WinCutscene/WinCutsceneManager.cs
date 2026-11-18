using UnityEngine;
using UnityEngine.Playables;

public class WinCutsceneManager : MonoBehaviour
{
    public PlayableDirector winCutscene;

    void Start()
    {
        winCutscene.Play();
    }
}
