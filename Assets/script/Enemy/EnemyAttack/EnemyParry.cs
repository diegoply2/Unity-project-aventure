using UnityEngine;
using System.Collections;

public class EnemyParry : MonoBehaviour
{
    [SerializeField] private float parryChance = 0.5f;  // Probabilité de parade
    [SerializeField] private float parryDuration = 1f;  // Durée de la parade

    private Animator animator;
    private bool isParrying = false;

    private EnemyAttackSound enemyParrySound;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyParrySound = GetComponent<EnemyAttackSound>();
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision détectée avec : " + other.gameObject.name);

        if (other.CompareTag("PlayerSword"))
        {
            Debug.Log("L'ennemi a bien détecté PlayerSword !");
            AttaqueScript playerAttackScript = other.GetComponentInParent<AttaqueScript>();
            ParadeScript playerParadeScript = other.GetComponentInParent<ParadeScript>();

            if (playerAttackScript != null && playerParadeScript != null && !playerParadeScript.isParrying && !playerAttackScript.isAttacking)
            {
                if (Random.value < parryChance)
                {
                    Debug.Log("L'ennemi décide de parer !");
                    StartCoroutine(ParryRoutine());
                }
            }
        }
    }

    private IEnumerator ParryRoutine()
    {
        Debug.Log("Enemy commence à parer !");
        isParrying = true;
        animator.SetBool("EnemyParry", true);
        enemyParrySound?.EnemyPlayParrySound();

        yield return new WaitForSeconds(parryDuration);

        Debug.Log("Enemy arrête de parer !");
        isParrying = false;
        animator.SetBool("EnemyParry", false);
    }
}
