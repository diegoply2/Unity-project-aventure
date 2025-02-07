using UnityEngine;
using System.Collections;

public class EnemyParry : MonoBehaviour
{
    [SerializeField] private float parryChance = 1f;  // Probabilité de parade
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

    // Vérifie si l'ennemi entre en collision avec l'épée du joueur
    if (other.CompareTag("PlayerSword"))
    {
        Debug.Log("L'ennemi a bien détecté PlayerSword !");

        // Ici, on ne vérifie plus si le joueur attaque ou pare.
        // L'ennemi peut parer s'il veut, en fonction de la probabilité
        if (Random.value < parryChance)
        {
            Debug.Log("L'ennemi décide de parer !");
            StartCoroutine(ParryRoutine());
        }
    }
}

    private IEnumerator ParryRoutine()
{
    // Log quand l'ennemi commence à parer
    Debug.Log("L'ennemi a commencé à parer !");
    
    // L'ennemi reste en parade pendant la durée définie par `parryDuration`
    isParrying = true;
    animator.SetBool("EnemyParry", true);
    enemyParrySound?.EnemyPlayParrySound();

    // Attend la durée de la parade avant de la terminer
    yield return new WaitForSeconds(parryDuration);

    // Log quand l'ennemi arrête de parer
    Debug.Log("L'ennemi a arrêté de parer !");
    
    // Fin de la parade
    isParrying = false;
    animator.SetBool("EnemyParry", false);
}

}
