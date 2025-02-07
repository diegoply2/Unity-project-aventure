using UnityEngine;
using System.Collections;

public class EnemyParry : MonoBehaviour
{
    public GameObject player;  // Référence à l'objet du joueur
    public float parryChance = 0.5f;  // 50% de chance de parer
    public float parryDuration = 1f;  // Durée de la parade

    private Animator animator;
    private bool isParrying = false;

    // Référence au script EnemyAttackSound pour jouer les sons
    private EnemyAttackSound enemyAttackSoundScript;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Récupérer la référence au script EnemyAttackSound
        enemyAttackSoundScript = GetComponent<EnemyAttackSound>();
        if (enemyAttackSoundScript == null)
        {
            Debug.LogError("Le script EnemyAttackSound n'est pas attaché à l'ennemi.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet qui entre en collision est l'épée du joueur
        if (other.CompareTag("PlayerSword"))
        {
            // Tirage aléatoire pour déterminer si l'ennemi va parer
            if (Random.value < parryChance)  // 50% de chance de parer
            {
                // L'ennemi décide de parer
                isParrying = true;
                animator.SetBool("EnemyParry", true);

                // Ajouter un délai pour arrêter la parade après un certain temps
                StartCoroutine(StopParryAfterDelay(parryDuration));  // Parer pendant 1 seconde

                // Joue le son de parade si l'ennemi parre
                if (enemyAttackSoundScript != null)
                {
                    enemyAttackSoundScript.EnemyPlayParrySound();
                }

                Debug.Log("L'ennemi a paré l'attaque !");
            }
            else
            {
                Debug.Log("L'ennemi n'a pas paré et a reçu l'attaque !");
                // Si l'ennemi ne parre, on peut infliger des dégâts ou gérer d'autres actions
            }
        }
    }

    // Coroutine pour arrêter la parade après un délai
    private IEnumerator StopParryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Arrêter la parade et réinitialiser l'animation
        isParrying = false;
        animator.SetBool("EnemyParry", false);
    }
}
