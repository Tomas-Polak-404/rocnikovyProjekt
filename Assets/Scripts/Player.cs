using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Nutné pro práci se scénami

public class Player : MonoBehaviour
{
    public float hp = 100; // Zdraví hráče
    public float maxHealth;
    public Image healthBar;

    private void Start()
    {
        maxHealth = hp;
    }

    private void Update()
    {
        
        healthBar.fillAmount = Mathf.Clamp(hp / maxHealth, 0, 1);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Zkontrolujeme, zda se hráč dotkl objektu s tagem "portal"
        if (other.CompareTag("Portal"))
        {
            // Spustíme přechod na scénu "lvl2v2"
            SceneManager.LoadScene("lvl2v2");
        }
    }

    // Metoda pro přijetí poškození
    public void TakeDamage(int damage)
    {

        hp -= damage; // Odečtení poškození od zdraví hráče
        if (hp < 0)
        {
            hp = 0; // Zajišťuje, že hráč nemůže mít méně než 0 HP
        }
    }
}
