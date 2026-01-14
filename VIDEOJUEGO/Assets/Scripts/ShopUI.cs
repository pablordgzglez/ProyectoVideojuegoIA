using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopPanel;

    [Header("Costs")]
    public int costHeal = 50;
    public int costDamage = 150;
    public int costSpeed = 150;

    [Header("Heal")]
    public float healAmount = 30f;

    [Header("Upgrades")]
    public float damageIncrease = 5f;
    public float speedMultiplier = 1.10f;

    private Player player;
    private Shooter shooter;
    private FirstPersonController fpc;

    private bool isOpen;

    void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    public bool IsOpen() => isOpen;

    public void Open(Player p, Shooter s, FirstPersonController controller)
    {
        player = p;
        shooter = s;
        fpc = controller;

        isOpen = true;

        if (shopPanel != null)
            shopPanel.SetActive(true);

        // Pausa el juego
        Time.timeScale = 0f;

        // Cursor libre para UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        isOpen = false;

        if (shopPanel != null)
            shopPanel.SetActive(false);

        // Reanuda el juego
        Time.timeScale = 1f;

        // Cursor FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BuyHealth()
    {
        if (player == null || PointsManager.Instance == null) return;
        if (!PointsManager.Instance.TrySpend(costHeal)) return;

        player.Heal(healAmount);
    }

    public void BuyDamage()
    {
        if (shooter == null || PointsManager.Instance == null) return;
        if (!PointsManager.Instance.TrySpend(costDamage)) return;

        shooter.damage += damageIncrease;
    }

    public void BuySpeed()
    {
        if (fpc == null || PointsManager.Instance == null) return;
        if (!PointsManager.Instance.TrySpend(costSpeed)) return;

        fpc.walkSpeed *= speedMultiplier;
        fpc.sprintSpeed *= speedMultiplier;
    }
}
