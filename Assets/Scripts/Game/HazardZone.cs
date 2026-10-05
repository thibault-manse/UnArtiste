using UnityEngine;

public class HazardZone : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float tickRate = 0.45f;

    private float nextTick;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < nextTick)
        {
            return;
        }

        if (other.TryGetComponent(out PlayerHealth health))
        {
            nextTick = Time.time + tickRate;
            health.TakeDamage(damage);
        }
    }
}
