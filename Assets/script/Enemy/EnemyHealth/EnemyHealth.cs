using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
{
    if (isDead) return;

    currentHealth -= amount;

    // Afficher combien de points de santé l'ennemi perd
    Debug.Log("L'ennemi a perdu " + amount + " points de santé. Santé restante : " + currentHealth);

    if (currentHealth <= 0)
    {
        Die();
    }
}

    private void Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");  // Joue une animation de mort si vous en avez une
        }
        // Vous pouvez aussi détruire l'objet ou désactiver l'ennemi après un délai
        Destroy(gameObject, 2f);
    }
}

