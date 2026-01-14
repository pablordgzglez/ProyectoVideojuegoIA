using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public ParticleSystem muzzleParticles;
    public AudioSource shotAudio;

    [Header("Weapon Stats")]
    public float damage = 30f;
    public float range = 120f;

    [Header("Fire Settings")]
    public float fireRate = 0.09f; // AKM ~ 600-700 RPM
    public bool automatic = true;

    [Header("Recoil")]
    public float recoilKick = 1.2f;
    public float recoilReturnSpeed = 18f;

    [Header("FX")]
    [Tooltip("Prefab de partículas de sangre. Se instancia en el punto de impacto si el hit es zombie.")]
    public ParticleSystem bloodFxPrefab;
    public float bloodFxLifeSeconds = 2f;

    private float nextFireTime;
    private float recoilOffset;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        bool wantsFire = automatic
            ? Input.GetMouseButton(0)
            : Input.GetMouseButtonDown(0);

        if (wantsFire && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Shoot();
            ApplyRecoil();
            PlayMuzzle();
            PlaySound();
        }
    }

    void LateUpdate()
    {
        if (playerCamera == null) return;

        recoilOffset = Mathf.Lerp(recoilOffset, 0f, recoilReturnSpeed * Time.deltaTime);

        Vector3 rot = playerCamera.transform.localEulerAngles;
        float x = rot.x;
        if (x > 180f) x -= 360f;

        x -= recoilOffset * Time.deltaTime;
        playerCamera.transform.localEulerAngles = new Vector3(x, rot.y, rot.z);
    }

    void Shoot()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, range, ~0, QueryTriggerInteraction.Ignore))
        {
            ZombieHealth zombie = hit.collider.GetComponentInParent<ZombieHealth>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
                SpawnBlood(hit);
            }
        }
    }

    void SpawnBlood(RaycastHit hit)
    {
        if (bloodFxPrefab == null) return;

        // Orientación: sale "desde" el zombie hacia fuera
        Quaternion rot = Quaternion.LookRotation(hit.normal);

        ParticleSystem fx = Instantiate(bloodFxPrefab, hit.point, rot);
        fx.Play();
        Destroy(fx.gameObject, bloodFxLifeSeconds);
    }

    void ApplyRecoil()
    {
        recoilOffset += recoilKick;
    }

    void PlayMuzzle()
    {
        if (muzzleParticles != null)
            muzzleParticles.Play();
    }

    void PlaySound()
    {
        if (shotAudio != null)
            shotAudio.Play();
    }
}
