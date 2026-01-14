using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance { get; private set; }

    public int points { get; private set; }

    private UIManager ui;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ui = FindFirstObjectByType<UIManager>();
        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0) return;
        points += amount;
        UpdateUI();
    }

    public bool TrySpend(int cost)
    {
        if (cost <= 0) return true;
        if (points < cost) return false;

        points -= cost;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (ui == null) ui = FindFirstObjectByType<UIManager>();
        if (ui != null) ui.UpdatePoints(points);
    }
}
