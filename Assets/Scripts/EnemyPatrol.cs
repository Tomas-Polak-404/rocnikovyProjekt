using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 5f;

    private Transform target;
    private SpriteRenderer spriteRenderer;

    private bool isDead = false; // Přidáme proměnnou pro sledování, zda je goblin mrtvý

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = pointA.transform;
    }
   
    void Update()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Body A a B nejsou správně nastaveny.");
            return;
        }

        // Přidáme podmínku, která zastaví pohyb goblina, pokud je mrtvý
        if (!isDead)
        {
            if (Vector2.Distance(transform.position, target.position) < 1)
            {
                ChangeTarget();
            }

            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Otáčení sprite podle směru pohybu
            if (transform.position.x < target.position.x)
            {
                spriteRenderer.flipX = false; // Normální směr
            }
            else
            {
                spriteRenderer.flipX = true; // Zrcadlový směr
            }
        }
    }

    void ChangeTarget()
    {
        if (target == pointA.transform)
        {
            target = pointB.transform;
        }
        else
        {
            target = pointA.transform;
        }
    }

    // Metoda, která nastaví stav goblina na mrtvý
    public void SetDead()
    {
        isDead = true;
        // Zastavit pohyb goblina
        speed = 0f;

    }

}
