using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class Player : MonoBehaviour
{
    public float hp = 100; // Zdraví hráče
    public float maxHealth;
    public Image healthBar;

    public Image gemBar; // Referenci na Image komponentu GemBar
    private int gemCount = 0;
    private int totalGems = 3; // Celkový počet drahokamů

    public GameObject gameOverScreen; // Reference na Game Over obrazovku
    private PlayerMovement playerMovement;
    private Animator anim;



    private bool isTakingDamage = false;


    private void Start()
    {
        anim = GetComponent<Animator>();
        maxHealth = hp;
    }

    private void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(hp / maxHealth, 0, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Zkontrolujeme, zda se hráč dotkl objektu s tagem "Portal"
        if (other.CompareTag("Portal") && gemCount >= totalGems)
        {
            // Spustíme přechod na scénu "lvl2v2"
            SceneManager.LoadScene("lvl2v2");
        }

        // Zkontrolujeme, zda se hráč dotkl objektu s tagem "Gem"
        if (other.CompareTag("Gem"))
        {
            gemCount++;
            UpdateGemBar();
            Destroy(other.gameObject); // Znič drahokam
        }

        // Zkontrolujeme, zda se hráč dotkl objektu s tagem "Void"
        if (other.CompareTag("Void"))
        {
            // Nastavení HP na 0
            hp = 0;
            healthBar.fillAmount = 0;

            // Zobrazení Game Over obrazovky
            gameOverScreen.SetActive(true);

            // Deaktivace pohybu hráče
            GetComponent<PlayerMovement>().enabled = false;
            DestroyWithTag("Bar");

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Traps") && !isTakingDamage)
        {
            StartCoroutine(TakeDamageCoroutine());
        }
    }
    private IEnumerator TakeDamageCoroutine()
    {
        isTakingDamage = true;
        TakeDamage(25);
        yield return new WaitForSeconds(1.0f);
        isTakingDamage = false;
    }
    public void GameOver()
    {
        hp = 0; // Zajišťuje, že hráč nemůže mít méně než 0 HP

        anim.SetBool("isPlayerDead", true);
        // Zastavení veškerého pohybu hráče
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        GetComponent<PlayerMovement>().enabled = false;
        // Zobrazení Game Over obrazovky
        gameOverScreen.SetActive(true);
        DestroyWithTag("Bar");
    }


    private void UpdateGemBar()
    {
        float fillAmount = (float)gemCount / totalGems;
        gemBar.fillAmount = fillAmount;
    }

    // Metoda pro přijetí poškození
    public void TakeDamage(int damage)
    {
        hp -= damage; // Odečtení poškození od zdraví hráče
        if (hp <= 0)
        {
            GameOver();
        }
    }
    void DestroyWithTag(string destroyTag)
    {
        GameObject[] destroyObject;
        destroyObject = GameObject.FindGameObjectsWithTag(destroyTag);
        foreach (GameObject oneObject in destroyObject)
            Destroy(oneObject);
    }
}





