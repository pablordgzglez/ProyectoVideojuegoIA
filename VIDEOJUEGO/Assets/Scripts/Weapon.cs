using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public string weaponName = "AKM";
    public int damage = 30;
    public float fireRate = 0.1f;
    public float range = 120f;

    [Header("References")]
    public Transform muzzle;   // arrastra aquí tu objeto "Muzzle"

    float nextFireTime;

    public bool CanFire() => Time.time >= nextFireTime;

    public void ConsumeShot()
    {
        nextFireTime = Time.time + fireRate;
    }
}
