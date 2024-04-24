using UnityEngine;

public class HealthUI : MonoBehaviour
{
        public int maxHealth = 100; // Maximální počet HP
        public int currentHealth; // Aktuální počet HP
        public GameObject gameOverCanvas; // Objekt canvasu "Game Over"

        void Start()
        {
            currentHealth = maxHealth; // Nastavení počátečního HP
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Goblin") // Pokud se dotkne goblika
            {
                currentHealth -= 5; // Odečtení 5 HP
                Debug.Log("Ztraceno 5 HP! Zbývá " + currentHealth + " HP.");

                if (currentHealth <= 0) // Pokud je HP 0 nebo menší
                {
                    gameOverCanvas.SetActive(true); // Zobrazení canvasu "Game Over"
                    Debug.Log("Hráč zemřel!");
                }
            }
        }

}
