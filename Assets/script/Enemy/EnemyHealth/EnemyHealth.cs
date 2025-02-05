using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;       // Santé maximale de l'ennemi
    public float currentHealth;          // Santé actuelle de l'ennemi

    private void Start()
    {
        // Initialisation de la santé actuelle à la santé maximale
        currentHealth = maxHealth;
    }

    // Méthode pour infliger des dégâts à l'ennemi
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Réduire la santé actuelle

        // Vérifier que la santé ne soit pas inférieure à 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Vérifier si l'ennemi est mort
        if (currentHealth <= 0)
        {
            Die(); // Appeler la méthode de mort de l'ennemi
        }
    }

    // Méthode pour gérer la mort de l'ennemi
    private void Die()
    {
        // Vous pouvez ajouter des effets ici, comme des animations, des sons, etc.
        Debug.Log("L'ennemi est mort !");
        Destroy(gameObject); // Détruire l'ennemi après sa mort
    }
}
