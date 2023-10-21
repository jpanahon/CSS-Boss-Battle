using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
