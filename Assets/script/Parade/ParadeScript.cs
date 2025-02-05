using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ParadeScript : MonoBehaviour
{
    private PlayerControls playerControls; // Référence aux contrôles personnalisés
    private Animator animator; // Référence à l'Animator
    public bool isParrying = false; // Indicateur pour vérifier si une parade est en cours

    private InputAction parryAction; // Action pour gérer la parade
    private CharacterControllerWithCamera characterController; // Référence au script de mouvement
    private PlayerHealth playerHealth; // Référence à PlayerHealth

    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Enable(); // Activer les actions

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("L'Animator n'a pas été trouvé sur l'objet.");
        }

        parryAction = playerControls.Player.Parade;
        if (parryAction == null)
        {
            Debug.LogError("L'action 'Parade' n'a pas été définie dans PlayerControls.");
        }

        // Référence au script de mouvement du personnage
        characterController = GetComponent<CharacterControllerWithCamera>();
        if (characterController == null)
        {
            Debug.LogError("Le CharacterControllerWithCamera n'a pas été trouvé sur l'objet.");
        }

        // Référence à PlayerHealth
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("Le script PlayerHealth n'a pas été trouvé sur l'objet.");
        }

        // Utilisation de "started" pour détecter la touche maintenue
        parryAction.started += ctx => StartParry();
        parryAction.canceled += ctx => EndParry(); // Quand le bouton est relâché, on arrête la parade
    }

    public void StartParry()
    {
        if (isParrying) return;

        isParrying = true;

        // Mettre à jour IsParrying dans PlayerHealth
        if (playerHealth != null)
        {
            playerHealth.IsParrying = true; // Déclare que le joueur est en train de parer
        }

        // Reste du code de la parade...
        if (animator != null)
        {
            animator.SetBool("Parade", true);
        }
    }

    public void EndParry()
    {
        if (!isParrying) return;

        isParrying = false;

        // Mettre à jour IsParrying dans PlayerHealth
        if (playerHealth != null)
        {
            playerHealth.IsParrying = false; // Fin de la parade
        }

        // Reste du code pour réactiver les mouvements etc...
        if (animator != null)
        {
            animator.SetBool("Parade", false);
        }
    }

    void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }
}
