using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp = 100; // Zdraví hráče

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
