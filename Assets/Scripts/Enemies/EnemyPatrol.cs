using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 3f;

    private Transform target;
    private SpriteRenderer spriteRenderer;

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
}
