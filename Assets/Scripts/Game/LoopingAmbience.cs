using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopingAmbience : MonoBehaviour
{
    [SerializeField] private float minPitch = 0.92f;
    [SerializeField] private float maxPitch = 1.04f;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(minPitch, maxPitch);
        source.loop = true;
        source.Play();
    }
}
