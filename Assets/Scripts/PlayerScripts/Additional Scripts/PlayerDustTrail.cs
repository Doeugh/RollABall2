using UnityEngine;

public class PlayerDustTrail : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private ParticleSystem freezeParticles;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0f, -0.8f, 0f);
    [SerializeField] private float groundCheckRadius = 0.25f;

    [SerializeField] private float minimumSpeed = 1f;
    [SerializeField] private float maximumSpeed = 8f;

    [SerializeField] private float minimumEmission = 2f;
    [SerializeField] private float maximumEmission = 25f;

    [SerializeField] private int freezeParticleCount = 15;

    [SerializeField] private Vector3 particleOffset = new Vector3(0f, -0.8f, 0f);

    private ParticleSystem.EmissionModule emission;

    private void Awake()
    {
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }

        if (dustParticles == null)
        {
            dustParticles = GetComponentInChildren<ParticleSystem>();
        }

        if (dustParticles == null)
        {
            enabled = false;
            return;
        }

        emission = dustParticles.emission;
        emission.enabled = true;
        emission.rateOverTime = 0f;

        dustParticles.Play();

        if (freezeParticles != null)
        {
            freezeParticles.Stop();
        }
    }

    private void Update()
    {
        if (playerRigidbody == null)
        {
            return;
        }

        Vector3 checkPosition = transform.position + groundCheckOffset;

        bool grounded = Physics.CheckSphere(
            checkPosition,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        Vector3 velocity = playerRigidbody.linearVelocity;
        velocity.y = 0f;

        float currentSpeed = velocity.magnitude;

        if (!grounded || currentSpeed < minimumSpeed)
        {
            emission.rateOverTime = 0f;
            return;
        }

        float speedPercentage = Mathf.InverseLerp(
            minimumSpeed,
            maximumSpeed,
            currentSpeed
        );

        float emissionRate = Mathf.Lerp(
            minimumEmission,
            maximumEmission,
            speedPercentage
        );

        emission.rateOverTime = emissionRate;
    }

    private void LateUpdate()
    {
        if (dustParticles != null)
        {
            dustParticles.transform.position = transform.position + particleOffset;
            dustParticles.transform.rotation = Quaternion.identity;
        }

        if (freezeParticles != null)
        {
            freezeParticles.transform.position = transform.position + particleOffset;
            freezeParticles.transform.rotation = Quaternion.identity;
        }
    }

    public void TriggerFreezeEffect()
    {
        if (freezeParticles == null)
        {
            return;
        }

        freezeParticles.Emit(freezeParticleCount);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 checkPosition = transform.position + groundCheckOffset;

        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
}
