using UnityEngine;
using System.Collections;

public class MysteryBoxEffect : MonoBehaviour
{
    [Header("References")]
    public InventorySystem inventory;

    [Header("Required Item")]
    public ItemData mysteryBoxItem;

    [Header("Fish Walking")]
    public GameObject walkingFishPrefab;
    public Transform fishSpawnPoint;
    public Transform fishTargetPoint;

    [Header("Money")]
    public int moneyPerTick = 5;
    public float moneyTickInterval = 1f;

    [Header("Spawn")]
    public float fishSpawnInterval = 2f;

    private bool isActive = false;
    private Coroutine moneyRoutine;
    private Coroutine fishRoutine;

    private void Awake()
    {
        if (inventory == null)
            inventory = GetComponent<InventorySystem>();
    }

    public void UseMysteryBox()
    {
        if (isActive)
        {
            Debug.Log("Mystery effect is already active.");
            return;
        }

        if (inventory == null)
            inventory = GetComponent<InventorySystem>();

        if (inventory == null)
        {
            Debug.LogWarning("MysteryBoxEffect: No InventorySystem.");
            return;
        }

        if (mysteryBoxItem == null)
        {
            Debug.LogWarning("MysteryBoxEffect: MysteryBoxItem missing.");
            return;
        }

        if (!inventory.HasItem(mysteryBoxItem, 1))
        {
            Debug.Log("You do not have Mystery Box.");
            return;
        }

        isActive = true;

        Debug.Log("Mystery Box activated. Fish are going to work.");

        moneyRoutine = StartCoroutine(MoneyRoutine());
        fishRoutine = StartCoroutine(FishRoutine());
    }

    private IEnumerator MoneyRoutine()
    {
        while (isActive)
        {
            GameManager.Instance.AddMoney(moneyPerTick);
            yield return new WaitForSeconds(moneyTickInterval);
        }
    }

    private IEnumerator FishRoutine()
    {
        while (isActive)
        {
            SpawnWalkingFish();
            yield return new WaitForSeconds(fishSpawnInterval);
        }
    }

    private void SpawnWalkingFish()
    {
        if (walkingFishPrefab == null)
        {
            Debug.LogWarning("MysteryBoxEffect: WalkingFishPrefab missing.");
            return;
        }

        if (fishSpawnPoint == null)
        {
            Debug.LogWarning("MysteryBoxEffect: FishSpawnPoint missing.");
            return;
        }

        GameObject fish = Instantiate(
            walkingFishPrefab,
            fishSpawnPoint.position,
            fishSpawnPoint.rotation
        );

        WalkingFish walking = fish.GetComponent<WalkingFish>();

        if (walking != null && fishTargetPoint != null)
        {
            walking.target = fishTargetPoint;
        }
    }

    public void StopEffect()
    {
        isActive = false;

        if (moneyRoutine != null)
            StopCoroutine(moneyRoutine);

        if (fishRoutine != null)
            StopCoroutine(fishRoutine);
    }
}