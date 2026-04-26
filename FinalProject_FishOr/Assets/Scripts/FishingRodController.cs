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
        if (!inventory.HasItem(basicBaitItem, 1))
        {
            Debug.Log("There is no more bait.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, castDistance, waterLayer))
        {
            inventory.RemoveItem(basicBaitItem, 1);
            StartCoroutine(FishingRoutine());
        }
    }

    IEnumerator FishingRoutine()
    {
        isFishing = true;
        Debug.Log("Reeling in the line...");

        yield return new WaitForSeconds(Random.Range(1.5f, 3.5f));

        Debug.Log("The fish has taken the bait and has been successfully pulled up!");
        SpawnFishOnShore();

        isFishing = false;
    }

    void SpawnFishOnShore()
    {
        if (groundFishPrefab != null && shoreSpawnPoint != null)
        {
            Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            Instantiate(groundFishPrefab, shoreSpawnPoint.position + offset, Quaternion.identity);
        }
    }
}