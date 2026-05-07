using UnityEngine;

public class WeaponFishingController : MonoBehaviour
{
    [Header("Explosion Effect")]
    public GameObject explosionEffectPrefab;

    [Header("References")]
    public Camera playerCamera;
    public InventorySystem inventory;

    [Header("Item Requirements")]
    public ItemData smgItem;
    public ItemData smgAmmoItem;
    public ItemData grenadeItem;
    public ItemData rpgItem;
    public ItemData rpgAmmoItem;

    [Header("Water Detection")]
    public LayerMask waterLayer;
    public float weaponRange = 60f;

    [Header("Fish Spawn")]
    public FishData[] availableFish;
    public Transform shoreSpawnPoint;

    [Header("Projectile Prefabs")]
    public GameObject grenadeProjectilePrefab;
    public GameObject rpgProjectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("SMG Settings")]
    public float smgFireCooldown = 0.08f;

    [Header("Throw Settings")]
    public float grenadeThrowForce = 12f;
    public float rpgShootForce = 25f;

    private float nextSMGFireTime = 0f;

    private void Awake()
    {
        AutoFindReferences();
    }

    private void AutoFindReferences()
    {
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        if (inventory == null)
            inventory = GetComponent<InventorySystem>();

        if (projectileSpawnPoint == null && playerCamera != null)
            projectileSpawnPoint = playerCamera.transform;
    }

    public void UseSMG()
    {
        AutoFindReferences();

        if (Time.time < nextSMGFireTime)
            return;

        nextSMGFireTime = Time.time + smgFireCooldown;

        if (!HasItem(smgItem, 1))
        {
            Debug.Log("You do not have SMG.");
            return;
        }

        if (!HasItem(smgAmmoItem, 1))
        {
            Debug.Log("No SMG ammo.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, weaponRange))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.5f);

            bool hitWater = ((1 << hit.collider.gameObject.layer) & waterLayer) != 0;

            if (hitWater)
            {
                inventory.RemoveItem(smgAmmoItem, 1);

                FishData fish = RollFishEqualChance();
                SpawnFishNearShore(fish);

                Debug.Log("SMG hit water. One fish spawned.");
            }
            else
            {
                Debug.Log("SMG hit something, but it is not water: " + hit.collider.name);
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * weaponRange, Color.red, 0.5f);
            Debug.Log("SMG did not hit anything.");
        }
    }

    public void ThrowGrenade()
    {
        AutoFindReferences();

        if (!HasItem(grenadeItem, 1))
        {
            Debug.Log("You do not have grenade.");
            return;
        }

        if (grenadeProjectilePrefab == null)
        {
            Debug.LogWarning("Grenade projectile prefab missing.");
            return;
        }

        inventory.RemoveItem(grenadeItem, 1);

        GameObject obj = Instantiate(
            grenadeProjectilePrefab,
            projectileSpawnPoint.position + playerCamera.transform.forward * 0.8f,
            Quaternion.identity
        );

        ExplosiveFishingProjectile projectile = obj.GetComponent<ExplosiveFishingProjectile>();

        if (projectile != null)
        {
            projectile.Setup(
                this,
                10,
                15
            );
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 force = playerCamera.transform.forward * grenadeThrowForce + Vector3.up * 3f;
            rb.AddForce(force, ForceMode.Impulse);
        }

        Debug.Log("Grenade thrown.");
    }

    public void FireRPG()
    {
        AutoFindReferences();

        if (!HasItem(rpgItem, 1))
        {
            Debug.Log("You do not have RPG.");
            return;
        }

        if (!HasItem(rpgAmmoItem, 1))
        {
            Debug.Log("No RPG ammo.");
            return;
        }

        if (rpgProjectilePrefab == null)
        {
            Debug.LogWarning("RPG projectile prefab missing.");
            return;
        }

        inventory.RemoveItem(rpgAmmoItem, 1);

        GameObject obj = Instantiate(
            rpgProjectilePrefab,
            projectileSpawnPoint.position + playerCamera.transform.forward * 1f,
            Quaternion.LookRotation(playerCamera.transform.forward)
        );

        ExplosiveFishingProjectile projectile = obj.GetComponent<ExplosiveFishingProjectile>();

        if (projectile != null)
        {
            projectile.Setup(
                this,
                20,
                25
            );
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(playerCamera.transform.forward * rpgShootForce, ForceMode.Impulse);
        }

        Debug.Log("RPG fired.");
    }

    private bool HasItem(ItemData item, int amount)
    {
        if (inventory == null || item == null)
            return false;

        return inventory.HasItem(item, amount);
    }

    public void SpawnExplosionFish(int minAmount, int maxAmount)
    {
        int amount = Random.Range(minAmount, maxAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            FishData fish = RollFishEqualChance();
            SpawnFishNearShore(fish);
        }

        Debug.Log($"Explosion spawned {amount} fish.");
    }

    private FishData RollFishEqualChance()
    {
        if (availableFish == null || availableFish.Length == 0)
        {
            Debug.LogWarning("WeaponFishingController has no available fish.");
            return null;
        }

        int index = Random.Range(0, availableFish.Length);
        return availableFish[index];
    }

    private void SpawnFishNearShore(FishData fishData)
    {
        if (fishData == null || fishData.fishPrefab == null)
        {
            Debug.LogWarning("FishData or fish prefab missing.");
            return;
        }

        if (shoreSpawnPoint == null)
        {
            Debug.LogWarning("No ShoreSpawnPoint connected.");
            return;
        }

        Vector3 offset = new Vector3(
            Random.Range(-3f, 3f),
            0,
            Random.Range(-3f, 3f)
        );

        GameObject fishObj = Instantiate(
            fishData.fishPrefab,
            shoreSpawnPoint.position + offset,
            Quaternion.identity
        );

        FishGroundPickup pickup = fishObj.GetComponent<FishGroundPickup>();

        if (pickup != null)
        {
            pickup.fishData = fishData;
            pickup.fishAmount = 1;
        }
    }
}