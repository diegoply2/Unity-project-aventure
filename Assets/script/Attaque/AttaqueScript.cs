using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class AttaqueScript : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator animator;
    public bool isAttacking = false;
    public bool isParrying = false; // Indicateur pour savoir si on est en train de parer

    private InputAction attackAction;
    private InputAction parryAction;

    public List<string> attackAnimations = new List<string> { "Attack1", "Attack2", "Attack3" };
    private AttackSound attackSoundScript;

    public GameObject sword;

    private PlayerHealth playerHealth;  // Déclarez playerHealth
    public delegate void AttackStartedEventHandler(bool isAttacking);  // Ajout du paramètre
public static event AttackStartedEventHandler OnAttackStarted;  // Déclaration de l'événement


    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
        animator = GetComponent<Animator>();

        attackAction = playerControls.Player.Attack;
        attackAction.started += ctx => StartAttack();

        parryAction = playerControls.Player.Parade;
        parryAction.started += ctx => StartParry();

        attackSoundScript = GetComponent<AttackSound>();

        // Initialiser playerHealth ici
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    public void StartAttack()
{
    if (isAttacking) return;
    isAttacking = true;

    // Ajoutez ceci pour mettre à jour l'état de l'attaque
    if (playerHealth != null)
    {
        playerHealth.isAttacking = true;
    }

    string randomAttackAnimation = attackAnimations[Random.Range(0, attackAnimations.Count)];
    animator.SetBool(randomAttackAnimation, true);

    attackSoundScript?.PlayAttackSound();

    // Activer le collider de l'épée
    if (sword != null)
        sword.GetComponent<Collider>().enabled = true;

    // Lancer l'événement d'attaque, en indiquant que l'attaque est en cours
    OnAttackStarted?.Invoke(true);  // Passez 'true' pour signifier que l'attaque a commencé

    StartCoroutine(ResetAttackBoolAfterDelay(2f));
}

    private IEnumerator ResetAttackBoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);

        isAttacking = false;

        // Désactiver le collider de l'épée après l'attaque
        if (sword != null)
            sword.GetComponent<Collider>().enabled = false;

        // Réinitialiser l'état de l'attaque dans PlayerHealth
        if (playerHealth != null)
        {
            playerHealth.isAttacking = false;
        }
    }

    public void StopAttack()  // Nouvelle méthode pour arrêter l'attaque
    {
        isAttacking = false;

        // Arrêter l'animation d'attaque si nécessaire
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);

        // Désactiver le collider de l'épée immédiatement
        if (sword != null)
            sword.GetComponent<Collider>().enabled = false;

        // Réinitialiser l'état de l'attaque dans PlayerHealth
        if (playerHealth != null)
        {
            playerHealth.isAttacking = false;
        }
    }

    public void StartParry()
    {
        // Si une attaque est en cours, on ne peut pas parer
        if (isAttacking) return;

        // Si le joueur n'est pas en train de parer, on lance la parade
        isParrying = true;
        attackSoundScript?.PlayParrySound();

        // Réinitialisation de l'état de la parade après 1 seconde
        StartCoroutine(ResetParryBoolAfterDelay(1f));
    }

    private IEnumerator ResetParryBoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isParrying = false; // Fin de la parade après un délai
    }

    void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();  // Désactiver les contrôles du joueur lorsque le script est désactivé
        }
        if (attackAction != null)
        {
            attackAction.Disable();
        }
        if (parryAction != null)
        {
            parryAction.Disable();
        }
    }

    // Cette méthode sera appelée lors de la collision de l'attaque avec un ennemi
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            // Vérifie si l'attaque touche un objet avec le tag "Enemy"
            if (other.CompareTag("Enemy"))
            {
                // Cette partie n'est plus utilisée, les dégâts sont maintenant gérés dans SwordScript
            }
        }
    }
}
