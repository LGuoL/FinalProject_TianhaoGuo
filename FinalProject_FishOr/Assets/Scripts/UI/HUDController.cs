using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI bucketText;
    public TextMeshProUGUI inventoryText;

    private InventorySystem inventory;
    private BucketSystem bucket;

    void Update()
    {
        FindPlayerReferences();

        if (GameManager.Instance != null)
        {
            if (orderText != null)
                orderText.text = $"Order: {GameManager.Instance.currentOrderSubmitted}/{GameManager.Instance.currentOrderRequired}";

            if (moneyText != null)
                moneyText.text = $"Money: ${GameManager.Instance.money}";

            if (dayText != null)
                dayText.text = $"Day: {GameManager.Instance.currentDay}";
        }

        if (bucketText != null && bucket != null)
        {
            bucketText.text = $"Bucket: {bucket.currentFishCount} fish";
        }

        if (inventoryText != null)
        {
            if (inventory != null)
            {
                inventoryText.text = $"Inventory Slots: {inventory.slots.Count}/{inventory.maxSlots}";
            }
            else
            {
                inventoryText.text = "Inventory Slots: Missing";
            }
        }
    }

    private void FindPlayerReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        inventory = player.GetComponent<InventorySystem>();
        bucket = player.GetComponent<BucketSystem>();
    }
}