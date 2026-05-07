using UnityEngine;

public class ExplosiveFishingProjectile : MonoBehaviour
{
    public LayerMask waterLayer;

    [Header("Explosion")]
    public GameObject explosionEffectPrefab;
    public float lifeTime = 5f;
    public float explosionDelayAfterHit = 0.05f;
    public float explosionEffectLifetime = 2f;

    private WeaponFishingController owner;
    private int minFish;
    private int maxFish;
    private bool hasExploded = false;
    private Vector3 explosionPoint;

    public void Setup(WeaponFishingController controller, int minAmount, int maxAmount)
    {
        owner = controller;
        minFish = minAmount;
        maxFish = maxAmount;

        if (owner != null)
        {
            waterLayer = owner.waterLayer;
            explosionEffectPrefab = owner.explosionEffectPrefab;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded)
            return;

        bool hitWater = ((1 << collision.gameObject.layer) & waterLayer) != 0;

        if (hitWater)
        {
            ContactPoint contact = collision.contacts[0];
            explosionPoint = contact.point;

            Invoke(nameof(Explode), explosionDelayAfterHit);
        }
    }

    private void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                explosionEffectPrefab,
                explosionPoint,
                Quaternion.identity
            );

            Destroy(effect, explosionEffectLifetime);
        }

        if (owner != null)
        {
            owner.SpawnExplosionFish(minFish, maxFish);
        }

        Destroy(gameObject);
    }
}