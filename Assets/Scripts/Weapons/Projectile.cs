using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifeTime = 1.8f;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Fire(Vector2 direction)
    {
        direction = direction.sqrMagnitude > 0.01f ? direction.normalized : Vector2.right;
        body.linearVelocity = direction * speed;
        transform.right = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth _))
        {
            return;
        }

        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
