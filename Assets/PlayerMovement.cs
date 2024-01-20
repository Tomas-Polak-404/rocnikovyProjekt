// Importov�n� nezbytn�ch knihoven a t��d pro pr�ci v Unity
using UnityEngine;

// Definice t��dy pro ��zen� pohybu hr��e
public class PlayerMovement : MonoBehaviour
{
    // Prom�nn� pro ulo�en� hodnoty vstupu z kl�vesnice pro horizont�ln� pohyb
    private float horizontal;

    // Booleovsk� prom�nn� pro sledov�n� sm�ru, sk�k�n� a pozemku
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool isGrounded = false;

    // Rigidbody2D komponenta hr��e, groundCheck transform a vrstva zem�
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Parametry pohybu
    [Header("Movement Parameters")]
    [SerializeField] private float speed = 7f;

    // Parametry skoku
    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 8f; // Skokov� s�la
    [SerializeField] private float gravity = 1f;   // Gravitace

    private void Update()
    {
        // Získání hodnoty horizontálního vstupu od hráče
        horizontal = Input.GetAxisRaw("Horizontal");
        
        // Aktualizace isGrounded pomocí metody IsGrounded()
        isGrounded = IsGrounded();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }
        // Otočení hráče podle směru pohybu
        Flip();

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
        // Zpracov�n� horizont�ln�ho pohybu
        HandleMovement();

        // Zpracov�n� skoku
        HandleJump();

        // Zpracov�n� gravitace
        HandleGravity();
    }

    // Metoda pro ��zen� horizont�ln�ho pohybu hr��e
    private void HandleMovement()
    {
        // Nastaven� rychlosti hr��e na z�klad� horizont�ln�ho vstupu
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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



