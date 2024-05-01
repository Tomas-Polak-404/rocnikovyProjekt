using UnityEngine;
using System.Collections;

public class Goblin : MonoBehaviour
{
    public GameManagerScript gameManager;
    private Animator anim;
    private EnemyPatrol enemyPatrol;
    public int damage = 5;
    public float attackRange = 5f;
    public bool isAttacking = false;

    private bool isDead;
    private bool canAttack = true;
    private Transform player;
    private bool playerInRange = false;

    public GameObject gameOverScreen;

    private void Start()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyPatrol.speed = 3f; // Set initial speed here
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
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        isAttacking = true;
        while (isAttacking)
        {
            if (player != null)
            {
                Player playerComponent = player.GetComponent<Player>();
                if (playerComponent != null)
                {
                    if (playerComponent.hp <= 0)
                    {
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
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            if (playerInRange)
            {
                StartCoroutine(ResumePatrolAfterDelay()); // Start the wait coroutine here
                anim.SetBool("isAttacking", false); // Disable "isAttacking" animation
            }
        }
    }
}
