using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Personnage ou objet à suivre
    public float distance = 5f; // Distance de la caméra par rapport au personnage
    public float height = 2f; // Hauteur de la caméra par rapport au personnage
    public float rotationSpeed = 5f; // Vitesse de rotation de la caméra autour du personnage
    public float smoothSpeed = 0.125f; // Vitesse de lissage du mouvement de la caméra

    private float currentRotation = 0f; // Rotation actuelle autour de l'axe Y
    private Vector3 currentVelocity = Vector3.zero; // Vitesse actuelle de la caméra pour le lissage

    private PlayerControls controls;
    private Transform cameraTransform; // Référence à la caméra

    void Awake()
    {
        // Assurez-vous que la caméra principale est assignée
        cameraTransform = Camera.main?.transform; // Si vous utilisez la caméra principale
        if (cameraTransform == null)
        {
            Debug.LogError("La caméra principale n'a pas été trouvée !");
        }
    }

    void OnEnable()
    {
        // Pas besoin de vérifier à nouveau ici si la caméra est nulle
        if (controls == null)
        {
            controls = new PlayerControls();
            controls.Enable(); // Assurez-vous que les contrôles sont initialisés
        }
    }

    void OnDisable()
    {
        // Désactiver les contrôles si définis
        if (controls != null)
        {
            controls.Disable();
        }
    }

    void Update()
    {
        if (target == null)
            return; // Si le target est null, ne rien faire.

        // Récupérer les entrées du stick droit (pour la rotation de la caméra)
        Vector2 cameraInput = controls.Player.Camera.ReadValue<Vector2>();
        float horizontalInput = cameraInput.x;
        float verticalInput = cameraInput.y;

        // Calcul de la nouvelle rotation en fonction de l'entrée (rotation autour de l'axe Y)
        currentRotation += horizontalInput * rotationSpeed * Time.deltaTime;

        // Calcul de la position désirée de la caméra derrière le joueur
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // Application du lissage de la position pour une caméra fluide
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // Faire tourner la caméra autour du personnage en fonction de l'entrée
        transform.RotateAround(target.position, Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

        // Toujours faire en sorte que la caméra regarde le personnage (en tenant compte de la hauteur)
        transform.LookAt(target.position + Vector3.up * height);
    }
}

