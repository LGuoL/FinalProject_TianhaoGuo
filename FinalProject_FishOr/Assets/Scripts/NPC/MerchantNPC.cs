using UnityEngine;

public class MerchantNPC : MonoBehaviour, IInteractable
{
    [Header("Sell Settings")]
    public int pricePerFish = 10;
    public BucketSystem playerBucket;

    private bool hasTalkedToday = false;

    public string GetInteractText()
    {
        if (GameManager.Instance.NeedDay3ShopDialogue())
        {
            return "Press E to talk to merchant (new products)";
        }

        if (playerBucket != null && playerBucket.currentFishCount > 0)
        {
            return $"Press E to sell {playerBucket.currentFishCount} fish";
        }

        return "Press E to talk / view today's order";
    }

    public void Interact()
    {
        if (GameManager.Instance.NeedDay3ShopDialogue())
        {
            hasTalkedToday = true;
            GameManager.Instance.UnlockSpecialShop();

            ShowDialogue("Merchant: New products are here! Normal fishing is not enough anymore. Try the SMG, grenades, RPG, and the mysterious box.");
            return;
        }

        if (playerBucket != null && playerBucket.currentFishCount > 0)
        {
            SellAllFish();
            return;
        }

        ShowDialogue($"Merchant: Today's order requires {GameManager.Instance.currentOrderRequired} fish in total.");
    }

    private void SellAllFish()
    {
        if (playerBucket == null)
        {
            ShowDialogue("Merchant: I cannot find your bucket.");
            return;
        }

        if (playerBucket.currentFishCount <= 0)
        {
            ShowDialogue("Merchant: Your bucket is empty.");
            return;
        }

        int totalFish = playerBucket.currentFishCount;

        int earned;

        // 如果你已经做了鱼价值系统，用这个：
        earned = playerBucket.currentFishValue > 0
            ? playerBucket.RemoveAllFishValue()
            : playerBucket.RemoveAllFish() * pricePerFish;

        GameManager.Instance.AddMoney(earned);

        ShowDialogue($"Merchant: Sold {totalFish} fish. You earned ${earned}.");
    }

    private void ShowDialogue(string message)
    {
        Debug.Log(message);

        if (DialogueUI.Instance != null)
            DialogueUI.Instance.ShowMessage(message);
    }

    public void ResetDailyDialogue()
    {
        hasTalkedToday = false;
    }
}