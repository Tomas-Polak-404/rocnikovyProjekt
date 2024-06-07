

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool isUnderObstacle = false;

    public Goblin goblin; // Reference na instanci třídy Goblin
    public Goblin musch;

    [SerializeField] private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    [Header("Movement Parameters")]
    [SerializeField] private float speed = 5f;
    private float baseSpeed;
    [SerializeField] private float runningSpeed = 7f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 1f;

    [SerializeField] public float crouchSpeed = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();
        baseSpeed = speed;
    }

    public void SetDead()
    {
        anim.SetBool("isPlayerDead", true);
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("isPlayerAttacking", false);
    }

    private void AttackGoblin()
    {
        if (goblin != null)
        {
            goblin.TakeDamage(25);
        }
        else if(musch != null)
        {
            musch.TakeDamage(25);
        }
        else
        {
            Debug.LogWarning("Reference na Goblina není nastavena!");
        }
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGrounded();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            anim.SetTrigger("JumpTrigger");
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            anim.SetBool("isPlayerAttacking", true);
            AttackGoblin();
            StartCoroutine(ResetAttackAnimation());
        }

        Flip();
        anim.SetBool("isJumping", !isGrounded);
    }

    private void FixedUpdate()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        CheckUnderObstacle();

        HandleCrouchInput();
        HandleMovement();
        HandleJump();
        HandleGravity();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isPlayerMoving = Mathf.Abs(horizontalInput) > 0.1f;

        float currentSpeed = isUnderObstacle || isCrouching ? crouchSpeed : speed;
        rb.velocity = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);

        anim.SetBool("isWalking", isPlayerMoving);

        bool isPlayerRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        anim.SetBool("isRunning", isPlayerRunning);
        if (isPlayerRunning)
        {
            speed = runningSpeed;
        }
        else
        {
            speed = baseSpeed;
        }
    }

    private void HandleCrouchInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                anim.SetBool("isCrouching", true);
                boxCollider.enabled = true;
                capsuleCollider.enabled = false;
            }
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
                anim.SetBool("isCrouching", false);
                boxCollider.enabled = true;
                capsuleCollider.enabled = true;
            }
        }
    }

    private void CheckUnderObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f);

        if (hit.collider != null && hit.collider.tag == "Obstacle")
        {
            isUnderObstacle = true;
        }
        else
        {
            isUnderObstacle = false;
        }
    }

    private void HandleJump()
    {
        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
        }
    }

    private void HandleGravity()
    {
        rb.velocity += Vector2.up * Physics2D.gravity.y * gravity * Time.fixedDeltaTime;
    }

    private bool IsGrounded()
    {
        anim.SetBool("isJumping", !isGrounded);
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
        return hit.collider != null;
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    #region Properties
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }
    #endregion
}
