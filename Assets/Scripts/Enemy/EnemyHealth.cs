using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 60;
    [SerializeField] private AudioSource deathAudio;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (deathAudio != null)
        {
            deathAudio.transform.SetParent(null);
            deathAudio.Play();
            Destroy(deathAudio.gameObject, 1.5f);
        }

        Destroy(gameObject);
    }
}
