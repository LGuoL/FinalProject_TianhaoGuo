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
        if (GameManager.Instance == null) return;

        orderText.text = $"Order: {GameManager.Instance.currentOrderSubmitted}/{GameManager.Instance.currentOrderRequired}";
        moneyText.text = $"Money: ${GameManager.Instance.money}";
        dayText.text = $"Day: {GameManager.Instance.currentDay}";
        bucketText.text = $"Bucket: {bucket.currentFishCount} fish";

        if (inventory != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Inventory:");
            foreach (var slot in inventory.slots)
            {
                sb.AppendLine($"{slot.item.itemName} x{slot.count}");
            }
            inventoryText.text = sb.ToString();
        }
    }
}