
using UnityEngine;

public class FireLight : MonoBehaviour
{
    public Light fireLight;

    public float minIntensity = 1.5f;
    public float maxIntensity = 3.5f;

    public float flickerSpeed = 8f;

    public Color fireColor = new Color(1f, 0.35f, 0.05f);

    private float noiseOffset;

    void Start()
    {
        if (fireLight == null)
        {
            fireLight = GetComponent<Light>();
        }

        noiseOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float flicker = Mathf.PerlinNoise(
            Time.time * flickerSpeed,
            noiseOffset
        );

        fireLight.intensity = Mathf.Lerp(
            minIntensity,
            maxIntensity,
            flicker
        );

        float colorVariation = Mathf.Lerp(0.7f, 1f, flicker);

        fireLight.color = fireColor * colorVariation;
    }
}

