using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject sword;  // Référence à l'objet de l'épée

    void Start()
    {
        // Si l'épée n'est pas assignée dans l'Inspector, on la trouve par tag
        if (sword == null)
        {
            sword = GameObject.FindGameObjectWithTag("PlayerSword");
        }

        if (sword != null)
        {
            Debug.Log("L'épée est assignée correctement.");
            // Applique directement la bonne rotation locale
            sword.transform.localRotation = Quaternion.Euler(45f, -45f, 80f);
        }
        else
        {
            Debug.LogError("L'épée n'est pas assignée !");
        }
    }

    // Assurez-vous que le collider de l'épée est un Trigger
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'épée touche un objet avec le tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Récupère la référence à EnemyHealth
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Inflige des dégâts à l'ennemi (vous pouvez ajuster le montant des dégâts ici)
                enemyHealth.TakeDamage(10f); // Remplacez 10f par la valeur appropriée
                Debug.Log("L'ennemi a perdu des points de santé !");
            }
        }
    }
}
