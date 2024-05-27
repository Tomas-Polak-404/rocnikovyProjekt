using System.Collections;
using UnityEngine;

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

    private float GoblinHealth = 100f; // Inicializace zdraví goblina na 100
    private float MaxGoblinHealth = 100f; // Max zdraví goblina na 100
    private bool isGoblinDead = false;

    [SerializeField] FloatingHealthBar healthBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyPatrol.speed = 3f; // Set initial speed here

        // Inicializace health baru
        healthBar.UpdateHealthBar(GoblinHealth, MaxGoblinHealth);
    }

    private void FixedUpdate()
    {
        if (player != null && !isDead && isAttacking)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, LayerMask.GetMask("Player"));

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        GoblinHealth -= damageAmount;
        Debug.Log("Goblin takes damage: " + damageAmount + ", Current Health: " + GoblinHealth);
        healthBar.UpdateHealthBar(GoblinHealth, MaxGoblinHealth);

        if (GoblinHealth <= 0)
        {
            Die();
        }
    }
    public void SetDead()
    {
        isGoblinDead = true;
        // Zavolejte metodu SetDead() i pro skript EnemyPatrol, abyste zastavili pohyb goblina
        EnemyPatrol enemyPatrol = GetComponent<EnemyPatrol>();
        if (enemyPatrol != null)
        {
            enemyPatrol.SetDead();
        }
    }
    private void Die()
    {
        isGoblinDead = true;
        enemyPatrol.SetDead();
        Debug.Log("Goblin died");
        anim.SetBool("isGoblinDead", true);
        // Můžete zde přidat další logiku pro smrt goblina, jako je zničení objektu po určité době
        Destroy(gameObject, 5f); // Zničit goblina po 2 sekundách
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
                    anim.SetBool("isGoblinAttacking", true);
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
                    anim.SetBool("isGoblinAttacking", false);
                    isAttacking = false;
                }
                playerInRange = false;
            }
        }

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
                        yield return new WaitForSeconds(1.0f);
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
            yield return new WaitForSeconds(0.667f);
        }

        isAttacking = false;
        canAttack = true;
    }

    private IEnumerator ResumePatrolAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        enemyPatrol.speed = 3f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            if (playerInRange)
            {
                anim.SetBool("isGoblinAttacking", true);
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
                anim.SetBool("isGoblinAttacking", false);
            }
        }
    }
}
