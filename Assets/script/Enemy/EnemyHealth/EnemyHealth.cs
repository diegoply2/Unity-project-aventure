using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // Vie maximale de l'ennemi
    [SerializeField] private float currentHealth;    // Vie actuelle de l'ennemi

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;  // Initialisation de la vie de l'ennemi
        animator = GetComponent<Animator>();  // Si vous avez un Animator pour l'ennemi
    }

    // Méthode pour recevoir des dégâts
    public void TakeDamage(float amount)
    {
        if (isDead) return;  // Si l'ennemi est déjà mort, il ne prend plus de dégâts

        currentHealth -= amount;  // Réduire la vie de l'ennemi

        if (currentHealth <= 0 && !isDead)
        {
            Die();  // Si la vie atteint 0, l'ennemi meurt
        }
    }

    // Méthode pour gérer la mort de l'ennemi
    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetBool("IsDead", true);  // Active l'animation de mort dans l'Animator
        }

        // Vous pouvez aussi ajouter des effets comme des particules de mort ou un son ici
        Destroy(gameObject, 2f);  // Délai de 2 secondes avant de détruire l'ennemi du jeu
    }

    // Méthode pour afficher l'état de la vie (facultatif, pour les débogages)
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Vie de l'ennemi : " + currentHealth);
    }
}
