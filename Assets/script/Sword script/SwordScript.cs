using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private AttaqueScript attaqueScript;  // Référence au script d'attaque

    void Start()
    {
        attaqueScript = GetComponentInParent<AttaqueScript>();

        if (attaqueScript == null)
        {
            Debug.LogError("AttaqueScript introuvable !");
        }

        // Assurez-vous que le collider est désactivé au départ
        GetComponent<Collider>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Vérifie si le joueur est en train d'attaquer avant de faire des dégâts
        if (attaqueScript != null && attaqueScript.isAttacking)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(5f);
                    Debug.Log("L'ennemi a perdu des points de santé !");
                }
            }
        }
    }
}
