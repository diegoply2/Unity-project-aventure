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
    
    //Debug.Log("Angle vers le joueur : " + angleToPlayer);

    Debug.DrawRay(transform.position + Vector3.up * 1.5f, directionToPlayer.normalized * viewDistance, Color.red);

    if (angleToPlayer <= viewAngle / 2f)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, directionToPlayer.normalized, out hit, viewDistance, whatIsPlayer))

        {
            Debug.Log("Raycast touche : " + hit.transform.name);

            if (hit.transform.CompareTag("Player"))
            {
                playerInSight = true;
                Debug.Log("Joueur détecté !");
                return;
            }
        }
    }

    playerInSight = false;
    Debug.Log("Joueur hors de vue.");
}




}
