using System.Collections;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    public Camera playerCamera;
    public float castDistance = 20f;
    public LayerMask waterLayer;

    public ItemData basicBaitItem;
    public InventorySystem inventory;

    [Header("Fish Spawn")]
    public GameObject groundFishPrefab;
    public Transform shoreSpawnPoint;

    private bool isFishing = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFishing)
        {
            TryFish();
        }
    }

    void TryFish()
    {
        if (inventory == null)
        {
            Debug.LogWarning("FishingRodController 没有连接 InventorySystem");
            return;
        }

        if (!inventory.HasItem(basicBaitItem, 1))
        {
            Debug.Log("没有鱼饵了");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, castDistance, waterLayer))
        {
            inventory.RemoveItem(basicBaitItem, 1);
            StartCoroutine(FishingRoutine());
        }
        else
        {
            Debug.Log("没有对准水面");
        }
    }

    IEnumerator FishingRoutine()
    {
        isFishing = true;
        Debug.Log("抛竿中...");

        yield return new WaitForSeconds(Random.Range(1.5f, 3.5f));

        Debug.Log("鱼上钩了，成功拉起！");
        SpawnFishOnShore();

        isFishing = false;
    }

    void SpawnFishOnShore()
    {
        if (groundFishPrefab == null)
        {
            Debug.LogWarning("没有设置 groundFishPrefab");
            return;
        }

        if (shoreSpawnPoint == null)
        {
            Debug.LogWarning("没有设置 shoreSpawnPoint");
            return;
        }

        Vector3 offset = new Vector3(
            Random.Range(-2f, 2f),
            0,
            Random.Range(-2f, 2f)
        );

        Instantiate(
            groundFishPrefab,
            shoreSpawnPoint.position + offset,
            Quaternion.identity
        );
    }
}