using UnityEngine;

public class BucketCollector : MonoBehaviour
{
    public Camera playerCamera;
    public BucketSystem bucketSystem;

    [Header("Collect Settings")]
    public float collectDistance = 3f;
    public float collectRadius = 2.5f;
    public LayerMask fishLayer;

    public void TryCollectFish()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera == null)
            {
                Debug.LogWarning("BucketCollector 没有 Camera");
                return;
            }
        }

        if (bucketSystem == null)
        {
            bucketSystem = GetComponent<BucketSystem>();
            if (bucketSystem == null)
            {
                Debug.LogWarning("BucketCollector 没有 BucketSystem");
                return;
            }
        }

        Vector3 center = playerCamera.transform.position + playerCamera.transform.forward * collectDistance;

        Collider[] hits = Physics.OverlapSphere(center, collectRadius, fishLayer);

        int collected = 0;

        foreach (Collider hit in hits)
        {
            FishGroundPickup fish = hit.GetComponent<FishGroundPickup>();

            if (fish == null)
            {
                fish = hit.GetComponentInParent<FishGroundPickup>();
            }

            if (fish != null)
            {
                bucketSystem.AddFish(fish.fishData, fish.fishAmount);
                collected += fish.fishAmount;
                Destroy(fish.gameObject);
            }
        }

        if (collected > 0)
        {
            Debug.Log($"范围吸入 {collected} 条鱼进桶");
        }
        else
        {
            Debug.Log("范围内没有可吸收的鱼");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.cyan;
        Vector3 center = playerCamera.transform.position + playerCamera.transform.forward * collectDistance;
        Gizmos.DrawWireSphere(center, collectRadius);
    }
}