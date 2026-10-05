using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase,
        Attack
    }

    [SerializeField] private float patrolSpeed = 1.1f;
    [SerializeField] private float chaseSpeed = 2.2f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float attackRange = 1.1f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float attackCooldown = 1.15f;
    [SerializeField] private Transform leftPatrolPoint;
    [SerializeField] private Transform rightPatrolPoint;

    private Rigidbody2D body;
    private Transform player;
    private State state;
    private float nextAttackTime;
    private bool patrolRight = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PlayerHealth health = FindAnyObjectByType<PlayerHealth>();
        if (health != null)
        {
            player = health.transform;
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            Patrol();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        state = distance <= attackRange ? State.Attack : distance <= detectionRange ? State.Chase : State.Patrol;

        if (state == State.Patrol)
        {
            Patrol();
        }
        else if (state == State.Chase)
        {
            Chase();
        }
        else
        {
            Attack();
        }
    }

    private void Patrol()
    {
        float direction = patrolRight ? 1f : -1f;
        body.linearVelocity = new Vector2(direction * patrolSpeed, body.linearVelocity.y);

        if (rightPatrolPoint != null && transform.position.x >= rightPatrolPoint.position.x)
        {
            patrolRight = false;
        }
        else if (leftPatrolPoint != null && transform.position.x <= leftPatrolPoint.position.x)
        {
            patrolRight = true;
        }

        Face(body.linearVelocity.x);
    }

    private void Chase()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        body.linearVelocity = new Vector2(direction * chaseSpeed, body.linearVelocity.y);
        Face(direction);
    }

    private void Attack()
    {
        body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
        Face(player.position.x - transform.position.x);

        if (Time.time >= nextAttackTime && player.TryGetComponent(out PlayerHealth health))
        {
            nextAttackTime = Time.time + attackCooldown;
            health.TakeDamage(damage);
        }
    }

    private void Face(float direction)
    {
        if (Mathf.Abs(direction) < 0.01f)
        {
            return;
        }

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
        transform.localScale = scale;
    }
}
