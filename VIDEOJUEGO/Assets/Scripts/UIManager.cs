using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtRound;
    public TextMeshProUGUI txtPoints;

    // NUEVO: banner grande centrado
    public TextMeshProUGUI txtRoundBanner;

    private Coroutine bannerCo;

    public void UpdateHealth(float health, float max)
    {
        if (txtHealth == null) return;
        txtHealth.text = "Vida: " + Mathf.CeilToInt(health) + "/" + Mathf.CeilToInt(max);
    }

    public void UpdateRound(int round)
    {
        if (txtRound == null) return;
        txtRound.text = "Ronda: " + round;
    }

    public void UpdatePoints(int points)
    {
        if (txtPoints == null) return;
        txtPoints.text = "Puntos: " + points;
    }

    // NUEVO
    public void ShowRoundBanner(string message, float seconds)
    {
        if (txtRoundBanner == null) return;

        if (bannerCo != null) StopCoroutine(bannerCo);
        bannerCo = StartCoroutine(BannerRoutine(message, seconds));
    }

    private IEnumerator BannerRoutine(string message, float seconds)
    {
        txtRoundBanner.gameObject.SetActive(true);
        txtRoundBanner.text = message;

        yield return new WaitForSeconds(seconds);

        txtRoundBanner.gameObject.SetActive(false);
        txtRoundBanner.text = "";
    }
}
