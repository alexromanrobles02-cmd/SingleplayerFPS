using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class WeaponController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private WeaponAnimationController weaponAnimationController;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem hitParticleSystem;

    private int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int chargerSize;

    [Header("Ammo Regeneration")]
    [SerializeField] private bool enableAmmoRegen = true;
    [SerializeField] private float ammoRegenInterval = 3f; // Tiempo en segundos para regenerar
    [SerializeField] private int ammoRegenAmount = 5;      // Cantidad de balas a regenerar
    [SerializeField] private int maxReserveCapacity = 60;  // Límite máximo de balas en la reserva
    private float regenTimer;


    /* 
     * 
     * Disparamos si tenemos balas (ammo)
     * Podemos recargar y se nos rellena hasta charger size o maxAmmo (si es mas pequeño que chargersize)
     * 
     */

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Start()
    {
        ammo = chargerSize;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetAmmo(ammo);
            UIManager.Instance.SetMaxAmmo(maxAmmo);
        }
    }

    void Update()
    {
        if (enableAmmoRegen && maxAmmo < maxReserveCapacity)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= ammoRegenInterval)
            {
                regenTimer = 0f; // Reiniciamos el temporizador
                maxAmmo = Mathf.Min(maxAmmo + ammoRegenAmount, maxReserveCapacity);
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.SetMaxAmmo(maxAmmo);
                }
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reload();
        }
    }

    void Shoot()
    {
        // LE TENGO QUE RESTAR UNA BALA. SI NO TIENE BALAS, NO DISPARO.
        if (ammo > 0)
        {
            ammo--;
            UIManager.Instance.SetAmmo(ammo);
            ShootRaycastFromCenter();
        }
    }

    private void Reload()
    {
        if (maxAmmo > 0 && ammo < chargerSize)
            weaponAnimationController.Reload();
    }

    void ShootRaycastFromCenter()
    {
        weaponAnimationController.Shoot();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f)); // Fixed: use 0.5f for z
        RaycastHit hit;

        float maxDistance = 100f;

        Vector3 endPoint;
        Vector3 hitDirection = Vector3.zero;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            endPoint = hit.point;

            // Calculate direction OPPOSITE to ray direction
            // This is from hit point back towards the camera
            hitDirection = -ray.direction.normalized;

            Debug.Log($"Hit: {hit.transform.name} | Ray Direction: {ray.direction} | Reverse Direction: {hitDirection}");

            if (hit.transform.CompareTag("Enemy"))
            {
                // Create rotation that faces the opposite direction of the ray
                Quaternion reverseRotation = Quaternion.LookRotation(hitDirection);

                // Pass the reverse rotation to the TakeDamage method
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(10, hit.point, reverseRotation);
                }
            }

            // Spawn hit particles at the impact point with reverse direction
            if (hitParticleSystem != null)
            {
                // Position at hit point
                hitParticleSystem.transform.position = hit.point;

                // Make particles go opposite to ray direction (back toward shooter)
                hitParticleSystem.transform.rotation = Quaternion.LookRotation(hitDirection);

                // Play the particle system
                hitParticleSystem.Play();

                Debug.DrawRay(hit.point, hitDirection * 5f, Color.green, 2f); // Visualize reverse direction
            }
        }
        else
        {
            endPoint = ray.origin + ray.direction * maxDistance;
        }

        // Visualize the ray for debugging
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 1f);

        /*
        // Draw in-game line
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, endPoint);
        */
    }

    //public void RequestReload()
    //{
    //    int bulletsToRemove = 0;
    //    if (maxAmmo < 
    //        chargerSize)
    //        bulletsToRemove = maxAmmo;
    //    else
    //        bulletsToRemove = chargerSize;

    //    ammo += bulletsToRemove;
    //    maxAmmo -= bulletsToRemove;
    //    UIManager.Instance.SetAmmo(ammo);
    //    UIManager.Instance.SetMaxAmmo(maxAmmo);
    //}

    public void RequestReload()
    {
        if (ammo >= chargerSize || maxAmmo <= 0)
            return;

        int spaceLeft = chargerSize - ammo;

        int bulletsToLoad = Mathf.Min(spaceLeft, maxAmmo);

        ammo += bulletsToLoad;
        maxAmmo -= bulletsToLoad;

        UIManager.Instance.SetAmmo(ammo);
        UIManager.Instance.SetMaxAmmo(maxAmmo);
    }

}
