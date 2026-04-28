using UnityEngine;

public class MerchantNPC : MonoBehaviour, IInteractable
{
    [Header("Sell Settings")]
    public int pricePerFish = 10;
    public BucketSystem playerBucket;

    private bool hasTalkedToday = false;

    public string GetInteractText()
    {
        if (GameManager.Instance.currentDay >= 3 && !hasTalkedToday)
        {
            return "Press E to talk to merchant(new products are available for display)";
        }

        if (playerBucket != null && playerBucket.currentFishCount > 0)
        {
            return "Press E to sell all the fish in the bucket";
        }

        return "Press E to talk to the merchant / view today's orders";
    }

    public void Interact()
    {
        if (GameManager.Instance.currentDay >= 3 && !hasTalkedToday)
        {
            hasTalkedToday = true;
            Debug.Log("商人：第3天开始有新货上架了，正常钓鱼已经不够用了。");
            return;
        }

        if (playerBucket != null && playerBucket.currentFishCount > 0)
        {
            SellAllFish();
            return;
        }

        Debug.Log($"商人：今日订单需要提交总共 {GameManager.Instance.currentOrderRequired} 条鱼。");
    }

    private void SellAllFish()
    {
        int totalFish = playerBucket.RemoveAllFish();

        if (totalFish <= 0)
        {
            Debug.Log("商人：你的桶里没有鱼。");
            return;
        }

        int earned = totalFish * pricePerFish;
        GameManager.Instance.AddMoney(earned);

        Debug.Log($"商人：收下了 {totalFish} 条鱼，你获得 {earned} 金币。");
    }

    public void ResetDailyDialogue()
    {
        hasTalkedToday = false;
    }
}