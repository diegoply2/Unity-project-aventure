using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Santé maximale du joueur
    public float currentHealth;    // Santé actuelle du joueur

    public bool IsParrying { get; set; } // Indicateur pour la parade

    void Start()
    {
        currentHealth = maxHealth; // Initialisation de la santé actuelle à la santé maximale
    }

    public void TakeDamage(float amount, bool isCritical)
    {
        if (!IsParrying)
        {
            currentHealth -= amount; // Soustraire les dégâts
        }
        else
        {
            currentHealth -= amount * 0.75f; // Réduction des dégâts de 25% en cas de parade
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Gérer la mort du joueur ici, par exemple en lançant une animation de mort ou en désactivant le joueur
        }

        Debug.Log("Santé actuelle : " + currentHealth);
    }
}
