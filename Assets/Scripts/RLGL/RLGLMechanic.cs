using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TrafficLightZone : MonoBehaviour
{
    public enum LightState { Green, SlowDown, Red }
    public LightState currentState = LightState.Green;

    public float minGreenTime = 4f;
    public float maxGreenTime = 8f;
    public float minRedTime = 3f;
    public float maxRedTime = 7f;
    public float slowDownDuration = 3.5f;

    public Light directionalLight;
    public Color greenColor = Color.green;
    public Color yellowColor = Color.yellow;
    public Color redColor = Color.red;

    public float movementThreshold = 5.0f;
    public string playerTag = "Player";

    public AudioSource audioSource;
    public AudioClip[] greenLightClips;
    public AudioClip[] yellowLightClips;

    private float stateTimer = 0f;
    private float stateDuration = 0f;

    private List<Rigidbody> playersInZone = new List<Rigidbody>();

    private void Start()
    {
        SetState(LightState.Green);
    }

    private void Update()
    {
        if (currentState == LightState.Red)
        {
            foreach (Rigidbody playerRb in playersInZone)
            {
                if (playerRb != null &&
                    playerRb.linearVelocity.magnitude > movementThreshold)
                {
                    SceneManager.LoadScene("YouDiedScreen");
                    return;
                }
            }
        }

        stateTimer += Time.deltaTime;

        while (stateTimer >= stateDuration)
        {
            if (currentState == LightState.Green)
            {
                SetState(LightState.SlowDown);
            }
            else if (currentState == LightState.SlowDown)
            {
                SetState(LightState.Red);
            }
            else if (currentState == LightState.Red)
            {
                SetState(LightState.Green);
            }
        }
    }

    private void SetState(LightState newState)
    {
        currentState = newState;
        stateTimer = 0f;

        if (newState == LightState.Green)
        {
            stateDuration = Random.Range(minGreenTime, maxGreenTime);
            SetLightColor(greenColor);
            PlayRandomClip(greenLightClips);
        }
        else if (newState == LightState.SlowDown)
        {
            stateDuration = slowDownDuration;
            SetLightColor(yellowColor);
            PlayRandomClip(yellowLightClips);
        }
        else if (newState == LightState.Red)
        {
            stateDuration = Random.Range(minRedTime, maxRedTime);
            SetLightColor(redColor);
        }
    }

    private void SetLightColor(Color c)
    {
        if (directionalLight != null)
        {
            directionalLight.color = c;
        }
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (audioSource == null || clips == null || clips.Length == 0)
            return;

        if (RLGLIntro.IsIntroPlaying)
        {
            StartCoroutine(PlayAfterIntro(clips));
            return;
        }

        int index = Random.Range(0, clips.Length);
        audioSource.clip = clips[index];
        audioSource.Play();
    }

    private System.Collections.IEnumerator PlayAfterIntro(AudioClip[] clips)
    {
        yield return new WaitUntil(() => !RLGLIntro.IsIntroPlaying);

        int index = Random.Range(0, clips.Length);
        audioSource.clip = clips[index];
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null && !playersInZone.Contains(rb))
        {
            playersInZone.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            playersInZone.Remove(rb);
        }
    }
}