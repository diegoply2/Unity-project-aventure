using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Vérifie si l'objet touché est un ennemi
        {
            Debug.Log("Ennemi touché !");
            
        }
    }
}