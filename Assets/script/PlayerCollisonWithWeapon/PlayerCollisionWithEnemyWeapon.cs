using UnityEngine;

public class PlayerCollisionWithEnemyWeapon : MonoBehaviour
{
    private EnemyAttack enemyAttackScript;

    void Start()
    {
        // Récupérer la référence à EnemyAttack
        enemyAttackScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifiez si l'objet touché est l'arme de l'ennemi
        if (other.CompareTag("EnemyWeapon"))
        {
            // Si l'arme touche le joueur, on appelle la méthode d'attaque de l'ennemi
            enemyAttackScript.HandlePlayerCollision(other);
        }
    }
}
