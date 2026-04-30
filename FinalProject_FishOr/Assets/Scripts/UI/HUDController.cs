using UnityEngine;
using TMPro;
using System.Text;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI bucketText;
    public TextMeshProUGUI inventoryText;

    public InventorySystem inventory;
    public BucketSystem bucket;

    void Update()
    {
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

        if (inventoryText != null && inventory != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Inventory: ");

            foreach (var slot in inventory.slots)
            {
                if (slot.item != null)
                {
                    sb.Append($"{slot.item.itemName} x{slot.count}   ");
                }
            }

            inventoryText.text = sb.ToString();
        }
    }
}