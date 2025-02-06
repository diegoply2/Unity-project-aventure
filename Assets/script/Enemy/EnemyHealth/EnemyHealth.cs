using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private bool isDead = false;
    private CharacterController characterController;  // Déclare la variable pour le CharacterController

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        
        // Récupère le CharacterController à partir de l'objet
        characterController = GetComponent<CharacterController>();

        // Vérifie si le CharacterController existe sur l'objet
        if (characterController == null)
        {
            Debug.LogError("Aucun CharacterController trouvé sur " + gameObject.name);
        }
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
        animator.SetBool("EnemyDie", true);  // Joue une animation de mort si vous en avez une
    }

    // Empêche d'autres interactions
    GetComponent<Collider>().enabled = false;  // Désactive les collisions

    // Désactive le CharacterController si présent
    if (characterController != null)
    {
        characterController.enabled = false;  // Empêche les déplacements du personnage
    }

    // Correction de la position pour éviter que le joueur reste suspendu dans l'air
    Vector3 currentPosition = transform.position;
    currentPosition.y = 0f;  // Ajuste la hauteur pour qu'il touche le sol
    transform.position = currentPosition;
}

}
