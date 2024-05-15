using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Goblin : MonoBehaviour
{
    public GameManagerScript gameManager;
    private Animator anim;
    private EnemyPatrol enemyPatrol;
    public int damage = 5;
    public float attackRange = 1.5f;
    public bool isAttacking = false;

    private bool isDead = false;
    private bool canAttack = true;
    private Transform player;
    private bool playerInRange = false;

    public GameObject gameOverScreen;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;





    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyPatrol.speed = 3f; // Set initial speed here
    }

    private void FixedUpdate()
    {
        if (player != null && !isDead && isAttacking)
        {
            // Zjistíme směr pohybu hráče
            Vector2 direction = (player.position - transform.position).normalized;

            // Zjistíme, zda je hráč ve směru pohybu goblina
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, LayerMask.GetMask("Player"));

            // Pokud hráč koliduje s goblinem, zastavíme pohyb goblina
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                rb.velocity = Vector2.zero;
            }
        }
    }


    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && !isDead)
            {
                if (!isAttacking)
                {
                    anim.SetBool("isAttacking", true);
                    enemyPatrol.speed = 0f;
                    if (canAttack)
                    {
                        StartCoroutine(AttackPlayer());
                    }
                }
                playerInRange = true;
            }
            else
            {
                if (isAttacking)
                {
                    StartCoroutine(ResumePatrolAfterDelay());
                    anim.SetBool("isAttacking", false);
                    isAttacking = false;
                }
                playerInRange = false;
            }
        }
        // Po celou dobu útoku udržujte polohu goblina na stejné výšce nad povrchem
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }


    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        isAttacking = true;
        while (isAttacking)
        {
            if (player != null && !isDead)
            {
                Player playerComponent = player.GetComponent<Player>();
                if (playerComponent != null)
                {
                    if (playerComponent.hp <= 0)
                    {
                        isDead = true;
                        playerMovement.SetDead();
                        yield return new WaitForSeconds(1.0f); // Wait for 1 second
                        gameOverScreen.SetActive(true);
                        isAttacking = false;
                    }
                    else
                    {
                        playerComponent.TakeDamage(damage);
                        Debug.Log("Hráč má " + playerComponent.hp + " HP.");
                    }
                }
            }
            yield return new WaitForSeconds(0.667f); // počkat čas délky animace útoku goblina

        }

        isAttacking = false; // Reset isAttacking when the loop ends
        canAttack = true;
    }

    private IEnumerator ResumePatrolAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second
        enemyPatrol.speed = 3f;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //rb.isKinematic = true;
            player = other.transform;
            if (playerInRange)
            {
                anim.SetBool("isAttacking", true); // Enable "isAttacking" animation
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            if (!playerInRange)
            {
                //rb.isKinematic = false;
                anim.SetBool("isAttacking", false); // Disable "isAttacking" animation
            }
        }
    }


}
