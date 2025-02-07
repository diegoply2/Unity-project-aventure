using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject sword;  // Référence à l'objet de l'épée
    public Transform playerHand;  // La main du joueur où l'épée doit être attachée
    public Vector3 offsetPosition;  // Décalage de la position de l'épée par rapport à la main
    public Vector3 rotationSpeed;  // Vitesse de rotation de l'épée sur les axes X, Y, Z
    public float smoothness = 5f;  // Fluidité du mouvement (le plus grand, plus fluide)

    private Quaternion targetRotation;
    private AttaqueScript attaqueScript;  // Référence au script AttaqueScript

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
            sword.transform.localRotation = Quaternion.Euler(45f, -45f, 80f);
        }
        else
        {
            Debug.LogError("L'épée n'est pas assignée !");
        }

        // Récupère la référence à AttaqueScript
        attaqueScript = GetComponentInParent<AttaqueScript>();
    }

    void Update()
    {
        if (sword != null && playerHand != null)
        {
            // Attacher l'épée à la main avec un décalage de position
            sword.transform.position = playerHand.position + offsetPosition;

            // Calculer la rotation désirée de l'épée basée sur les entrées de la souris
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed.x;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed.y;
            float rotationZ = Input.GetAxis("Fire3") * rotationSpeed.z;

            // Créer une nouvelle rotation basée sur les entrées de l'utilisateur
            targetRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

            // Appliquer la rotation avec une interpolation douce pour la fluidité
            sword.transform.rotation = Quaternion.Slerp(sword.transform.rotation, targetRotation, smoothness * Time.deltaTime);
        }
    }

    // Assurez-vous que le collider de l'épée est un Trigger
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'épée touche un objet avec le tag "Enemy" et si l'attaque est en cours
        if (other.CompareTag("Enemy") && attaqueScript.isAttacking)
        {
            // Récupère la référence à EnemyHealth
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Inflige des dégâts à l'ennemi (valeur aléatoire entre 2 et 8)
                float randomDamage = Random.Range(2f, 8f);  
                enemyHealth.TakeDamage(randomDamage); 
                Debug.Log("L'ennemi a perdu " + randomDamage + " points de santé !");
            }
        }
    }
}
