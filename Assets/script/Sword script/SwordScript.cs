using UnityEngine;
using UnityEngine.InputSystem;

public class SwordScript : MonoBehaviour
{
    private AttaqueScript attaqueScript;
    private PlayerControls playerControls;  // Référence au PlayerControls
    private Vector2 movementInput;  // Entrée de mouvement

    private float rotationSpeedX = 100f;
    private float rotationSpeedY = 150f;
    private float rotationSpeedZ = 200f;

    void Start()
    {
        attaqueScript = GetComponentInParent<AttaqueScript>();

        if (attaqueScript == null)
        {
            Debug.LogError("AttaqueScript introuvable !");
        }

        // Assurez-vous que le collider est désactivé au départ
        GetComponent<Collider>().enabled = false;

        // Récupérer la référence du PlayerControls
        playerControls = new PlayerControls();
        playerControls.Enable();

        // Vérification si PlayerControls est bien assigné
        if (playerControls == null)
        {
            Debug.LogError("PlayerControls introuvable ! Assurez-vous que le fichier d'actions est bien configuré.");
        }
    }

    void Update()
    {
        // Vérifie si l'action Move est bien définie
        if (playerControls != null)
        {
            // Lire la valeur de l'input de mouvement
            movementInput = playerControls.Player.Move.ReadValue<Vector2>();

            // Si l'attaque est en cours
            if (attaqueScript.isAttacking)
            {
                // Rotation sur l'axe X
                transform.Rotate(Vector3.right, movementInput.y * rotationSpeedX * Time.deltaTime);

                // Rotation sur l'axe Y
                transform.Rotate(Vector3.up, movementInput.x * rotationSpeedY * Time.deltaTime);

                // Rotation sur l'axe Z (exemple dynamique)
                float rotationInputZ = Mathf.Sin(Time.time);
                transform.Rotate(Vector3.forward, rotationInputZ * rotationSpeedZ * Time.deltaTime);
            }
        }
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

    void OnDisable()
    {
        // Désactive les contrôles lorsque l'objet est désactivé
        playerControls.Disable();
    }
}
