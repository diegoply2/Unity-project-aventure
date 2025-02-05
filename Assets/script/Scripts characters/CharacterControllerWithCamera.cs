using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerWithCamera : MonoBehaviour
{
    private PlayerControls playerControls; // Référence aux contrôles personnalisés

    // Variables de mouvement
    public float moveSpeed = 5f; // Vitesse de déplacement normale
    public float runSpeed = 10f; // Vitesse de course
    public float rotationSpeed = 700f; // Vitesse de rotation du personnage
    public float deadzone = 0.3f; // Valeur de la deadzone (vous pouvez ajuster cette valeur)

    // Variables de caméra
    public Transform cameraTransform; // Référence à la caméra pour calculer le mouvement relatif
    private Vector2 moveInput; // Entrée du stick gauche pour les mouvements
    private Vector2 lookInput; // Entrée du stick droit pour la rotation de la caméra

    private CharacterController characterController;
    private Vector3 velocity;
    private Animator animator; // Référence à l'Animator

    // Variables pour le saut
    public float jumpHeight = 2f; // Hauteur du saut
    public float gravityValue = -9.81f; // Valeur de la gravité
    private bool isJumping = false; // Indicateur pour savoir si le personnage est en train de sauter
    private bool isGrounded; // Indicateur si le personnage est au sol

    private InputAction jumpAction; // Action pour le saut
    private InputAction runAction; // Action pour courir (LeftShoulder dans PlayerControls)
    private InputAction attackAction; // Action pour attaquer

    public Vector2 smoothMoveInput; // Pour lisser les entrées de mouvement
    public bool isRunning = false; // Indicateur pour savoir si le personnage court
    public bool IsRun = false; // Booléen pour l'animation de la course

    private AttaqueScript attackScript; // Référence au script d'attaque
    private ParadeScript parryScript;

    void Awake()
    {
        // Vérifier la présence de la caméra principale
        cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("La caméra principale n'a pas été trouvée !");
        }

        playerControls = new PlayerControls();
        playerControls.Enable(); // Activer les actions d'entrée

        // Vérifier la présence du CharacterController, Animator et AttaqueScript
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Le CharacterController n'a pas été trouvé sur l'objet.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("L'Animator n'a pas été trouvé sur l'objet.");
        }

        attackScript = GetComponent<AttaqueScript>();
        if (attackScript == null)
        {
            Debug.LogError("Le script d'attaque n'a pas été trouvé sur l'objet.");
        }

        parryScript = GetComponent<ParadeScript>();
        if (parryScript == null)
        {
            Debug.LogError("Le script ParadeScript n'a pas été trouvé sur l'objet.");
        }


        // Obtenir les actions du player
        jumpAction = playerControls.Player.Jump;
        jumpAction.started += ctx => HandleJump();

        runAction = playerControls.Player.Run;
        runAction.started += ctx => StartRunning();
        runAction.canceled += ctx => StopRunning();

        attackAction = playerControls.Player.Attack;
    }

    void Update()
    {
        // Si le personnage attaque, ne pas gérer les mouvements
        if ((attackScript != null && attackScript.isAttacking) || (parryScript != null && parryScript.isParrying))
        {
            smoothMoveInput = Vector2.zero; // Désactiver le mouvement pendant l'attaque ou la parade
        }
        else
        {
            moveInput = playerControls.Player.Move.ReadValue<Vector2>();
            lookInput = playerControls.Player.Camera.ReadValue<Vector2>();
            ApplyDeadzone(ref moveInput);

            smoothMoveInput = Vector2.Lerp(smoothMoveInput, moveInput, 0.2f);
        if (moveInput == Vector2.zero)
            smoothMoveInput = Vector2.zero;
        }


        isGrounded = characterController.isGrounded;

        if (!isRunning && !isJumping && moveInput == Vector2.zero && attackScript != null && !attackScript.isAttacking)
        {
            if (attackAction.triggered)
            {
                Attack();
            }
        }

        MoveCharacter();
        RotateCharacterWithCamera();

        if (isGrounded)
        {
            velocity.y = -2f;
            if (isJumping)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
                isJumping = false;
                animator.SetBool("IsJumping", true);
            }
            animator.SetBool("IsJumping", false);
        }
        else
        {
            velocity.y += gravityValue * Time.deltaTime;
            animator.SetBool("IsJumping", true);
        }

        characterController.Move(velocity * Time.deltaTime);
        UpdateAnimations();
    }

    void MoveCharacter()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f; right.y = 0f; forward.Normalize(); right.Normalize();
        Vector3 moveDirection = (forward * smoothMoveInput.y + right * smoothMoveInput.x).normalized;

        float currentMoveSpeed = isRunning ? runSpeed : moveSpeed;
        characterController.Move(moveDirection * currentMoveSpeed * Time.deltaTime);
    }

    void RotateCharacterWithCamera()
    {
        if (lookInput.sqrMagnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(lookInput.x, lookInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void UpdateAnimations()
    {
        if (animator != null)
        {
            float moveX = smoothMoveInput.x;
            float moveY = smoothMoveInput.y;
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);

            bool isMovingForward = moveY > 0.1f;
            bool isMovingBackward = moveY < -0.1f;

            if (isGrounded)
            {
                if (isMovingForward || isMovingBackward)
                {
                    animator.SetBool("IsRun", IsRun);
                }
                else
                {
                    animator.SetBool("IsRun", false);
                }
            }
        }
    }

    private void StartRunning()
    {
        isRunning = true;
        IsRun = true;
    }

    private void StopRunning()
    {
        isRunning = false;
        IsRun = false;
    }

    private void ApplyDeadzone(ref Vector2 input)
    {
        if (input.magnitude < deadzone)
        {
            input = Vector2.zero;
        }
        else
        {
            input = input.normalized * ((input.magnitude - deadzone) / (1 - deadzone));
        }
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }
    }

    void Attack()
    {
        if (attackScript != null)
        {
            attackScript.StartAttack();
        }
    }

    void OnDisable()
{
    if (playerControls != null)
    {
        playerControls.Disable(); // Désactiver les entrées lorsque le script est désactivé
    }

    // Si vous avez d'autres objets comme attackScript ou des composants, vous pouvez également les vérifier ici.
    if (attackScript != null)
    {
        // Effectuer des actions de nettoyage supplémentaires pour l'attaque, si nécessaire.
    }

    
}

public void DisableMovement()
{
    Debug.Log("Mouvement désactivé");
    moveInput = Vector2.zero;
}

public void EnableMovement()
{
    Debug.Log("Mouvement réactivé");
    moveInput = playerControls.Player.Move.ReadValue<Vector2>();
}

}
