using UnityEngine;

public class SwordScript : MonoBehaviour
{
    // Déclarez la variable sword pour que l'épée puisse être assignée dans l'Inspector
    public GameObject sword;  // Référence à l'objet de l'épée

    void Start()
{
    // Si l'épée n'est pas assignée dans l'Inspector, on la trouve par tag
    if (sword == null)
    {
        sword = GameObject.FindGameObjectWithTag("PlayerSword"); // Utilisation du tag correct "PlayerSword"
    }

    if (sword != null)
    {
        Debug.Log("L'épée est assignée correctement.");
        // Manipuler l'épée (par exemple, lui donner une orientation)
        sword.transform.rotation = Quaternion.Euler(90f, 0f, 0f);  // Rotation vers le haut
    }
    else
    {
        Debug.LogError("L'épée n'est pas assignée !");
    }
}
    void Update()
    {
        // Exemple de manipulation de l'épée
        if (sword != null)
        {
            // Exemple : tourner l'épée pour la positionner vers le haut
            sword.transform.rotation = Quaternion.Euler(90f, 45f, 45f);  // Exemple de rotation vers le haut
        }
    }
}