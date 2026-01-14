using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float health;

    private UIManager ui;
    private GameOverManager gameOver;
    private bool dead;

    void Start()
    {
        health = maxHealth;
        dead = false;

        ui = FindFirstObjectByType<UIManager>();
        if (ui != null) ui.UpdateHealth(health, maxHealth);

        // lo buscamos una vez (más eficiente)
        gameOver = FindFirstObjectByType<GameOverManager>();
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;              // si ya está muerto, ignorar daño
        if (amount <= 0f) return;      // seguridad

        health -= amount;
        if (health < 0f) health = 0f;

        if (ui != null) ui.UpdateHealth(health, maxHealth);

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (dead) return;
        dead = true;

        Debug.Log("Player muerto");

        
        // Mostrar Game Over
        if (gameOver == null) gameOver = FindFirstObjectByType<GameOverManager>();
        if (gameOver != null)
            gameOver.ShowGameOver();
        else
            Debug.LogWarning("No existe GameOverManager en la escena.");
    }

    public bool IsDead()
    {
        return dead;
    }

    // Opcional: por si luego añades botiquines
    public void Heal(float amount)
    {
        if (dead) return;
        if (amount <= 0f) return;

        health += amount;
        if (health > maxHealth) health = maxHealth;

        if (ui != null) ui.UpdateHealth(health, maxHealth);
    }
}
