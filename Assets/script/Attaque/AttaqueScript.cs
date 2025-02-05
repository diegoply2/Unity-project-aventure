using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class AttaqueScript : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator animator;
    public bool isAttacking = false;
    public bool isParrying = false;

    private InputAction attackAction;
    private InputAction parryAction;
    private bool isRunning;

    public List<string> attackAnimations = new List<string> { "Attack1", "Attack2", "Attack3" };
    private AttackSound attackSoundScript;

    public GameObject sword; // Assigne l'épée dans l'Inspector

    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
        animator = GetComponent<Animator>();

        attackAction = playerControls.Player.Attack;
        attackAction.started += ctx => StartAttack();

        parryAction = playerControls.Player.Parade;
        parryAction.started += ctx => StartParry();

        attackSoundScript = GetComponent<AttackSound>();
    }

    public void StartAttack()
{
    if (isAttacking) return;
    isAttacking = true;

    string randomAttackAnimation = attackAnimations[Random.Range(0, attackAnimations.Count)];
    animator.SetBool(randomAttackAnimation, true);

    attackSoundScript?.PlayAttackSound();

    // Active le collider de l'épée
    if (sword != null)
        sword.GetComponent<Collider>().enabled = true;

    StartCoroutine(ResetAttackBoolAfterDelay(2f));
}

// Désactive le collider après l'attaque
private IEnumerator ResetAttackBoolAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    
    animator.SetBool("Attack1", false);
    animator.SetBool("Attack2", false);
    animator.SetBool("Attack3", false);
    
    isAttacking = false;

    // Désactiver le collider de l'épée après l'attaque
    if (sword != null)
        sword.GetComponent<Collider>().enabled = false;
}

    public void StartParry()
    {
        if (isParrying) return;
        isParrying = true;

        attackSoundScript?.PlayParrySound();
        StartCoroutine(ResetParryBoolAfterDelay(1f));
    }

    

    private IEnumerator ResetParryBoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isParrying = false;
    }

    void OnDisable()
    {
        playerControls?.Disable();
        attackAction?.Disable();
        parryAction?.Disable();
    }
}
