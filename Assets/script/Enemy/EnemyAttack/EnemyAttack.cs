using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float minAttackDelay = 0.5f;
    [SerializeField] private float maxAttackDelay = 1.5f;

    private bool isAttacking = false;
    private Transform player;
    private Animator animator;
    private ParadeScript playerParadeScript;
    private EnemyParry enemyParry;

    private EnemyAttackSound enemyAttackSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        playerParadeScript = player.GetComponent<ParadeScript>();

        enemyParry = GetComponent<EnemyParry>(); // Gestion de la parade déplacée vers EnemyParry
        enemyAttackSound = GetComponent<EnemyAttackSound>();
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
        isAttacking = true;
        float attackDelay = Random.Range(minAttackDelay, maxAttackDelay);
        yield return new WaitForSeconds(attackDelay);

        if (!enemyParry.IsParrying()) // Vérifie si l'ennemi ne pare pas avant d'attaquer
        {
            animator.SetBool("EnemyAttack", true);
            enemyAttackSound?.EnemyPlayAttackSound();

            yield return new WaitForSeconds(1f);

            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    if (playerParadeScript != null && playerParadeScript.isParrying && !playerHealth.isAttacking)
                    {
                        playerHealth.TakeDamage(Random.Range(2f, 5f) * 0.75f, false);
                    }
                    else
                    {
                        playerHealth.TakeDamage(Random.Range(2f, 5f), false);
                    }
                }
            }

            animator.SetBool("EnemyAttack", false);
        }

        isAttacking = false;
    }
}
