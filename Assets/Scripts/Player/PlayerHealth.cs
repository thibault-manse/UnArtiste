using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageInvulnerability = 0.75f;

    private int currentHealth;
    private float nextDamageTime;
    private PlayerController controller;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || Time.time < nextDamageTime)
        {
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);
        nextDamageTime = Time.time + damageInvulnerability;

        if (currentHealth <= 0)
        {
            controller?.LockControls(true);
            GameManager.Instance?.GameOver();
        }
    }

    public void RestoreFull()
    {
        currentHealth = maxHealth;
        nextDamageTime = 0f;
        controller?.LockControls(false);
    }
}
