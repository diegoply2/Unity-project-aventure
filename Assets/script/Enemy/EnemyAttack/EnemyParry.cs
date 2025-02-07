using UnityEngine;
using System.Collections;

public class EnemyParry : MonoBehaviour
{
    public GameObject player;  // Référence à l'objet du joueur
    public float parryChance = 1f;  // 50% de chance de parer
    public float parryDuration = 1f;  // Durée de la parade
    public float reactionDelay = 0.1f; // Délai avant que l'ennemi réagisse

    private Animator animator;
    private bool isParrying = false;
    private bool isAttackDetected = false;  // Nouveau flag pour savoir si une attaque a été détectée

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

        // Abonnez-vous à l'événement de début d'attaque du joueur
        AttaqueScript.OnAttackStarted += TryParry;
    }

    // Méthode pour tenter de parer dès que l'attaque commence
    private void TryParry(bool isAttacking)
    {
        if (isAttacking && !isParrying)  // Vérifiez si l'attaque a commencé et si l'ennemi ne parre pas déjà
        {
            isAttackDetected = true; // L'attaque a été détectée

            // Ajoutez un délai avant que l'ennemi ne commence à parer
            StartCoroutine(TryParryWithDelay());
        }
    }

    // Coroutine pour ajouter un délai avant que l'ennemi essaie de parer
    private IEnumerator TryParryWithDelay()
    {
        yield return new WaitForSeconds(reactionDelay);

        // Si une attaque a bien été détectée et l'ennemi n'est pas déjà en train de parer
        if (isAttackDetected && !isParrying)
        {
            // Tirage aléatoire pour déterminer si l'ennemi va parer
            if (Random.value < parryChance)  // 50% de chance de parer
            {
                // L'ennemi décide de parer
                isParrying = true;
                animator.SetBool("EnemyParry", true);  // Déclenche l'animation de parade

                // Joue immédiatement le son de parade
                if (enemyAttackSoundScript != null)
                {
                    enemyAttackSoundScript.EnemyPlayParrySound();
                }

                // Réinitialiser la parade après une certaine durée
                StartCoroutine(StopParryAfterDelay(parryDuration));  // Parer pendant 1 seconde

                Debug.Log("L'ennemi a paré l'attaque !");
            }
            else
            {
                Debug.Log("L'ennemi n'a pas paré et a reçu l'attaque !");
                // Si l'ennemi ne parre, tu peux infliger des dégâts ou gérer d'autres actions
            }

            // Réinitialisez la détection de l'attaque après la parade
            isAttackDetected = false;
        }
    }

    // Coroutine pour arrêter la parade après un délai
    private IEnumerator StopParryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Arrêter la parade et réinitialiser l'animation
        isParrying = false;
        animator.SetBool("EnemyParry", false);  // Arrêter l'animation de parade
    }

    void OnDestroy()
    {
        // S'assurer de se désinscrire de l'événement pour éviter des fuites de mémoire
        AttaqueScript.OnAttackStarted -= TryParry;
    }
}
