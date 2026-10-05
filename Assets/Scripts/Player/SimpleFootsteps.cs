using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleFootsteps : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float stepInterval = 0.42f;

    private AudioSource source;
    private float nextStep;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (controller == null || body == null || !controller.IsGrounded || Mathf.Abs(body.linearVelocity.x) < 0.8f)
        {
            return;
        }

        if (Time.time >= nextStep)
        {
            nextStep = Time.time + stepInterval;
            source.pitch = Random.Range(0.85f, 1.05f);
            source.Play();
        }
    }
}
