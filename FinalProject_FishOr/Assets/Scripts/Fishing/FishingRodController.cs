using System.Collections;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    public Camera playerCamera;
    public float castDistance = 20f;
    public LayerMask waterLayer;

    [Header("Inventory")]
    public InventorySystem inventory;

    [Header("Rod")]
    public RodData currentRod;

    [Header("Baits")]
    public BaitData currentBait;
    public BaitData basicBait;
    public BaitData betterBait;
    public BaitData bestBait;

    [Header("Fish")]
    public FishData[] availableFish;

    [Header("Fish Spawn")]
    public Transform shoreSpawnPoint;

    [Header("Timing")]
    public float minBaseBiteTime = 1.5f;
    public float maxBaseBiteTime = 3.5f;

    [Header("QTE")]
    public FishingQTE fishingQTE;

    private bool isFishing = false;
    private FishData pendingFish;

    private void Start()
    {
        if (currentBait == null)
            currentBait = basicBait;
    }

    public void SetBait(BaitData bait)
    {
        currentBait = bait;
    }

    public void SetRod(RodData rod)
    {
        currentRod = rod;
    }

    public void TryFish()
    {
        if (isFishing)
            return;

        if (inventory == null)
        {
            inventory = GetComponent<InventorySystem>();
            if (inventory == null)
            {
                Debug.LogWarning("No InventorySystem.");
                return;
            }
        }

        if (currentBait == null || currentBait.baitItem == null)
        {
            Debug.LogWarning("No bait selected.");
            return;
        }

        if (!inventory.HasItem(currentBait.baitItem, 1))
        {
            Debug.Log("No bait: " + currentBait.baitName);
            return;
        }

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera == null)
                return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, castDistance, waterLayer))
        {
            inventory.RemoveItem(currentBait.baitItem, 1);
            StartCoroutine(FishingRoutine());
        }
        else
        {
            Debug.Log("You are not aiming at water.");
        }
    }

    private IEnumerator FishingRoutine()
    {
        isFishing = true;

        float rodMultiplier = currentRod != null ? currentRod.biteTimeMultiplier : 1f;
        float baitMultiplier = currentBait != null ? currentBait.biteTimeMultiplier : 1f;

        float waitTime = Random.Range(minBaseBiteTime, maxBaseBiteTime);
        waitTime *= rodMultiplier;
        waitTime *= baitMultiplier;

        Debug.Log($"Fishing... Wait time: {waitTime:F1}s");

        yield return new WaitForSeconds(waitTime);

        pendingFish = RollFish();

        Debug.Log("Fish is biting! Start QTE.");

        StartFishingQTE();
    }

    private void StartFishingQTE()
    {
        if (fishingQTE == null)
        {
            fishingQTE = FindFirstObjectByType<FishingQTE>();
        }

        if (fishingQTE == null)
        {
            Debug.LogWarning("No FishingQTE found. Auto success.");
            OnQTEResult(true);
            return;
        }

        float qteDifficulty = currentRod != null ? currentRod.qteDifficultyMultiplier : 1f;

        fishingQTE.StartQTE(qteDifficulty, OnQTEResult);
    }

    private void OnQTEResult(bool success)
    {
        if (success)
        {
            Debug.Log("QTE success! Fish caught.");
            SpawnFishOnShore(pendingFish);
        }
        else
        {
            Debug.Log("QTE failed! Fish escaped.");
        }

        pendingFish = null;
        isFishing = false;
    }

    private FishData RollFish()
    {
        if (availableFish == null || availableFish.Length == 0)
        {
            Debug.LogWarning("No fish data assigned.");
            return null;
        }

        float rareBonus = currentBait != null ? currentBait.rareFishBonus : 0f;

        float totalWeight = 0f;

        for (int i = 0; i < availableFish.Length; i++)
        {
            totalWeight += GetModifiedWeight(availableFish[i], i, rareBonus);
        }

        float roll = Random.Range(0, totalWeight);
        float current = 0f;

        for (int i = 0; i < availableFish.Length; i++)
        {
            current += GetModifiedWeight(availableFish[i], i, rareBonus);

            if (roll <= current)
            {
                Debug.Log("Hooked: " + availableFish[i].fishName);
                return availableFish[i];
            }
        }

        return availableFish[0];
    }

    private float GetModifiedWeight(FishData fish, int index, float rareBonus)
    {
        if (fish == null)
            return 0f;

        float weight = fish.baseWeight;
        float rarityFactor = index;

        weight += rareBonus * rarityFactor * 10f;

        return Mathf.Max(0.1f, weight);
    }

    private void SpawnFishOnShore(FishData fishData)
    {
        if (fishData == null || fishData.fishPrefab == null)
        {
            Debug.LogWarning("FishData or fishPrefab missing.");
            return;
        }

        if (shoreSpawnPoint == null)
        {
            Debug.LogWarning("No ShoreSpawnPoint.");
            return;
        }

        Vector3 offset = new Vector3(
            Random.Range(-2f, 2f),
            0,
            Random.Range(-2f, 2f)
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