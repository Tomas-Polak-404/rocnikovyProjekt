/*
--------------------------Ročníková práce - Tomáš Polák---------------------


TODO: foreground, enemies, UI, lights, levels, žebřík,...
    
TODO: celkově upravit kód



*/




using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool isUnderObstacle = false;


    [SerializeField] private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    // Parametry pohybu
    [Header("Movement Parameters")]
    [SerializeField] private float speed = 5f;
    private float baseSpeed;
    [SerializeField] private float runningSpeed = 7f;

    // Parametry skoku
    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 8f; 
    [SerializeField] private float gravity = 1f;

    [SerializeField] public float crouchSpeed = 2f;


    void Start()
    {
        anim = GetComponent<Animator>();
        baseSpeed = speed;
    }


    private void Update()
    {
        // Získání hodnoty horizontálního vstupu od hráče
        horizontal = Input.GetAxisRaw("Horizontal");
        
        // Aktualizace isGrounded pomocí metody IsGrounded()
        isGrounded = IsGrounded();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            // Aktivovat Trigger pro skok
            anim.SetTrigger("JumpTrigger");
        }
        // Otočení hráče podle směru pohybu
        Flip();

        anim.SetBool("isJumping", !isGrounded);

        /*
        if (isGrounded)
        {
            Debug.Log("Jsem na zemi");
        }
        else
        {
            Debug.Log("Jsem ve vzduchu");
        }
        */
        // Metoda volan� v ka�d�m framu pro z�sk�n� vstupu od hr��e
        // Povol skok pouze pokud je hráč na zemi a hráč stiskne skokové tlačítko
    }
    

    // Metoda volan� v ka�d�m fixed framu pro zpracov�n� pohybu, skoku a gravitace
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

        // Pokud je pod překážkou nebo v pozici crouch, používej crouchSpeed, jinak používej normální speed
        float currentSpeed = isUnderObstacle || isCrouching ? crouchSpeed : speed;
        rb.velocity = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);

        // Nastavuje Animator parametr "isMoving" podle toho, zda se hráč pohybuje
        anim.SetBool("isWalking", isPlayerMoving);

        bool isPlayerRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        anim.SetBool("isRunning", isPlayerRunning);
        if(isPlayerRunning) {
            speed = runningSpeed;
        }
        else {
            speed = baseSpeed;
        }

    }






    private void HandleCrouchInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                // Přepni do crouch stavu a deaktivuj box collider u nohou
                isCrouching = true;
                anim.SetBool("isCrouching", true);
                boxCollider.enabled = true;
                capsuleCollider.enabled = false; // Možná chceš aktivovat circle collider, pokud má být používán
            }
        }
        else
        {
            if (isCrouching)
            {
                // Opuštění crouch stavu, aktivuj box collider u nohou
                isCrouching = false;
                anim.SetBool("isCrouching", false);
                boxCollider.enabled = true;
                capsuleCollider.enabled = true; // Aktivuj circle collider, pokud je to tvoje požadované chování
            }
        }
    }
    private void CheckUnderObstacle() //TODO: nefunguje 
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

    // Metoda pro zpracov�n� skoku hr��e
    private void HandleJump()
    {
        // Pokud hr�� stisknul skokov� tla��tko, nastav�me vertik�ln� rychlost na hodnotu skokov� s�ly a resetujeme isJumping
        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            isJumping = false;
        }
    }

    // Metoda pro zpracov�n� gravitace
    private void HandleGravity()
    {
        // Aplikace gravita�n�ho vlivu na hr��e
        rb.velocity += Vector2.up * Physics2D.gravity.y * gravity * Time.fixedDeltaTime;
    }

    // Metoda pro zji�t�n�, zda je hr�� na zemi pomoc� Raycast
    private bool IsGrounded()
    {
        /*
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector3.down, 0.2f, groundLayer);
        return hit.collider != null;
        */

        anim.SetBool("isJumping", !isGrounded);
        
        // Vytvo�en� raycastu sm��uj�c�ho dol�
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);

        // Vr�t�me true, pokud raycast narazil na n�co
        return hit.collider != null;
        
    }


    // Metoda pro oto�en� hr��e podle sm�ru pohybu
    private void Flip()
    {
        // Pokud hr�� m�n� sm�r pohybu, provede se oto�en� sprite hr��e
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }



    // Properties pro p��stup k hodnot�m prom�nn�ch ze zven��
    #region Properties

    // Rychlost pohybu
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // S�la skoku
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

    // Gravitace
    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }

    #endregion
}