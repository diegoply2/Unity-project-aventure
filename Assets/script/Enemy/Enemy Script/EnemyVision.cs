using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float viewDistance = 20f;
    public float viewAngle = 120f;
    public Transform player;
    public LayerMask whatIsPlayer;

    public bool playerInSight { get; private set; }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
{
    Vector3 directionToPlayer = player.position - transform.position;
    float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);

    // Affiche la direction du raycast
    Debug.DrawRay(transform.position, directionToPlayer.normalized * viewDistance, Color.red); // Affiche la direction de détection

 // Affiche l'angle entre l'ennemi et le joueur

    // Vérifiez si le joueur est dans l'angle de vue de l'ennemi
    if (angleToPlayer <= viewAngle / 2f)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, viewDistance, whatIsPlayer))
        {
            // Vérification du tag du joueur
            if (hit.transform.CompareTag("Player"))
            {
                playerInSight = true;
                Debug.Log("Joueur détecté !");
                return; // Le joueur est détecté, sortir de la fonction
            }
            else
            {
                Debug.Log("Raycast touche un autre objet: " + hit.transform.name);
            }
        }
    }
    playerInSight = false; // Le joueur n'est plus dans le champ de vision
    Debug.Log("Joueur hors de vue.");
}



}
