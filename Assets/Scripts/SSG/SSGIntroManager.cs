using UnityEngine;
using System.Collections;

public class SSGIntroManager : MonoBehaviour
{
    public PlayerController player;

    public SSQEnemyMovement[] enemies;

    public AudioSource introAudio;

    public AudioSource backgroundAudio;

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

        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        if (introAudio != null && introAudio.clip != null)
        {
            introAudio.Play();

            yield return new WaitWhile(() => introAudio.isPlaying);
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
