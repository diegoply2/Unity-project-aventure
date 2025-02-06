using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float minAttackDelay = 0.5f;
    [SerializeField] private float maxAttackDelay = 1.5f;
    [SerializeField] private float parryChance = 0.5f;  // Probabilité de parade

    private bool isAttacking = false;
    private Transform player;
    private Animator animator;
    private ParadeScript playerParadeScript; // Référence à ParadeScript

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Trouve le joueur
        animator = GetComponent<Animator>();
        

        // Obtenir la référence à ParadeScript du joueur
        playerParadeScript = player.GetComponent<ParadeScript>();
        
    }

    public void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackWithDelay());
        }
    }

    private IEnumerator AttackWithDelay()
{
    isAttacking = true;  // Indiquer que l'ennemi est en attaque

    float attackDelay = Random.Range(minAttackDelay, maxAttackDelay);
    yield return new WaitForSeconds(attackDelay);

    if (animator != null)
    {
        animator.SetBool("EnemyAttack", true); // Lancer l'attaque
    }

    // Attendre la durée de l'attaque pour simuler l'animation
    yield return new WaitForSeconds(1f); // Simule la durée de l'animation d'attaque

    // Vérification du contact avec le joueur et réduction des dégâts après l'attaque
    if (Vector3.Distance(transform.position, player.position) <= attackRange)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            if (playerParadeScript != null && playerParadeScript.isParrying && !playerHealth.isAttacking)  // Si le joueur est en train de parer et n'attaque pas
            {
                // Réduction des dégâts de 1/4 en cas de parade
                playerHealth.TakeDamage(Random.Range(2f, 5f) * 0.75f, false);  // Réduit les dégâts de 25%
            }
            else
            {
                // Applique des dégâts à la santé du joueur
                playerHealth.TakeDamage(Random.Range(2f, 5f), false);
            }
        }
    }

    // Réinitialisation du paramètre d'attaque dans l'Animator
    if (animator != null)
    {
        animator.SetBool("EnemyAttack", false);  // Fin de l'attaque
    }

    isAttacking = false;  // L'ennemi n'attaque plus
}

    // OnTriggerEnter corrigé pour appeler StartParry depuis ParadeScript
    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("PlayerSword")) // Vérifie si c'est bien l'épée du joueur
    {
        // Vérifiez maintenant l'état d'attaque dans AttaqueScript au lieu de ParadeScript
        AttaqueScript playerAttackScript = player.GetComponent<AttaqueScript>();

        if (playerAttackScript != null && !playerParadeScript.isParrying && !playerAttackScript.isAttacking)
        {
            if (Random.value < parryChance) // Si le nombre généré est inférieur à la probabilité de parade
            {
                playerParadeScript.StartParry(); // L'ennemi effectue une parade via ParadeScript
               
            }
            
        }
    }
}
}

