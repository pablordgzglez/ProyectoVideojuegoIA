using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenBloodEffect : MonoBehaviour
{
    [Header("References")]
    public Player player;             
    public Volume globalVolume;        

    [Header("Damage Flash (hit)")]
    public float hitVignetteIntensity = 0.45f;
    public float hitFadeInTime = 0.03f;
    public float hitHoldTime = 0.05f;
    public float hitFadeOutTime = 0.15f;

    [Header("Low Health (persistent)")]
    [Range(0f, 1f)]
    public float lowHealthThreshold = 0.35f;   // por debajo de 35% empieza el filtro
    public float lowHealthMaxVignette = 0.35f; // cuánto vignette máximo con poca vida
    public float lowHealthMaxTint = 0.35f;     // fuerza del tinte rojo (0-1)

    private Vignette vignette;
    private ColorAdjustments colorAdjust;
    private float lastHealth;
    private Coroutine hitRoutine;

    void Awake()
    {
        if (player == null) player = GetComponent<Player>();
        if (globalVolume == null) globalVolume = FindFirstObjectByType<Volume>();

        if (globalVolume == null || globalVolume.profile == null)
        {
            Debug.LogError("No encuentro Global Volume o no tiene Profile asignado.");
            enabled = false;
            return;
        }

        // Obtener overrides del profile
        if (!globalVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("El Volume Profile no tiene Vignette. Añádelo en el Global Volume.");
            enabled = false;
            return;
        }

        if (!globalVolume.profile.TryGet(out colorAdjust))
        {
            Debug.LogError("El Volume Profile no tiene Color Adjustments. Añádelo en el Global Volume.");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        if (player != null) lastHealth = player.health;
    }

    void Update()
    {
        if (globalVolume == null || globalVolume.profile == null)
            return;

        if (player == null) return;

        // Detectar daño: si baja la vida desde el frame anterior
        if (player.health < lastHealth)
        {
            TriggerHitFlash();
        }

        lastHealth = player.health;

        // Efecto constante por poca vida
        ApplyLowHealthEffect();
    }

    void TriggerHitFlash()
    {
        if (hitRoutine != null) StopCoroutine(hitRoutine);
        hitRoutine = StartCoroutine(HitFlashRoutine());
    }

    IEnumerator HitFlashRoutine()
    {
        // Asegurar color de bordes rojo
        vignette.color.value = new Color(0.65f, 0.05f, 0.05f, 1f);
        vignette.smoothness.value = 0.65f;

        float start = vignette.intensity.value;
        float t = 0f;

        // Fade in rápido
        while (t < hitFadeInTime)
        {
            t += Time.deltaTime;
            float a = (hitFadeInTime <= 0f) ? 1f : (t / hitFadeInTime);
            vignette.intensity.value = Mathf.Lerp(start, hitVignetteIntensity, a);
            yield return null;
        }

        // Hold
        if (hitHoldTime > 0f) yield return new WaitForSeconds(hitHoldTime);

        // Fade out
        float from = vignette.intensity.value;
        t = 0f;
        while (t < hitFadeOutTime)
        {
            t += Time.deltaTime;
            float a = (hitFadeOutTime <= 0f) ? 1f : (t / hitFadeOutTime);
            vignette.intensity.value = Mathf.Lerp(from, 0f, a);
            yield return null;
        }

        vignette.intensity.value = 0f;
        hitRoutine = null;
    }

    void ApplyLowHealthEffect()
    {
        float ratio = Mathf.Clamp01(player.health / player.maxHealth);

        if (ratio >= lowHealthThreshold)
        {
            // Sin filtro rojo constante
            colorAdjust.colorFilter.value = Color.white;
            return;
        }

        // 0..1 según cuánta vida falta por debajo del umbral
        float k = Mathf.InverseLerp(lowHealthThreshold, 0f, ratio);

        // Tinte rojo (Color Filter)
        // Mezcla blanco -> rojo suave
        Color targetTint = Color.Lerp(Color.white, new Color(1f, 0.3f, 0.3f, 1f), k * lowHealthMaxTint);
        colorAdjust.colorFilter.value = targetTint;

        // Vignette constante adicional (pero sin “pisar” el flash si está activo)
        if (hitRoutine == null)
        {
            vignette.color.value = new Color(0.65f, 0.05f, 0.05f, 1f);
            vignette.smoothness.value = 0.65f;
            vignette.intensity.value = Mathf.Lerp(0f, lowHealthMaxVignette, k);
        }
    }
}
