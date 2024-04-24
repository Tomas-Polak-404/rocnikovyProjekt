using UnityEngine;

public class Goblin : MonoBehaviour
{
    public GameObject gameOverText; // Objekt canvasu "Game Over"
    public int damage = 5; // Poškození způsobené goblinem


    // Metoda, která se zavolá, když se jiný objekt dotkne goblina
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Pokud se dotýkající objekt označuje jako hráč
        {
            Player player = other.GetComponent<Player>(); // Získání reference na skript hráče
            if (player != null) // Zkontroluje, jestli se hráč opravdu nachází na objektu
            {
                player.TakeDamage(damage); // Zavolání metody TakeDamage() na hráči s poškozením
                Debug.Log("Hráč má " + player.hp + " HP."); // Výpis aktuálního zdraví hráče do konzole

                if (player.hp <= 0)
                {
                    gameOverText.SetActive(true); // Zobrazení canvasu "Game Over"
                    Debug.Log("Game Over!"); // Výpis Game Over zprávy do konzole
                    // Zde by mělo následovat ukončení hry, pokud chceš, že hra se vypne
                    // Application.Quit(); // Toto ukončí hru (nefunguje v editoru Unity)
                }
            }
        }
    }
}
