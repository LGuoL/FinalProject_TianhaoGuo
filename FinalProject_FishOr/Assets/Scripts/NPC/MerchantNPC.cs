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
            return "Press E to talk to merchant (new products available)";
        }

        if (playerBucket != null && playerBucket.currentFishCount > 0)
        {
            return $"Press E to sell {playerBucket.currentFishCount} fish";
        }

        return "Press E to talk / view today's order";
    }

    public void Interact()
    {
        if (GameManager.Instance.currentDay >= 3 && !hasTalkedToday)
        {
            hasTalkedToday = true;
            ShowDialogue("Merchant: New products are available! Normal fishing is not enough anymore.");
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
        int totalFish = playerBucket.RemoveAllFish();

        if (totalFish <= 0)
        {
            ShowDialogue("Merchant: Your bucket is empty.");
            return;
        }

        int earned = totalFish * pricePerFish;
        GameManager.Instance.AddMoney(earned);

        ShowDialogue($"Merchant: Sold {totalFish} fish. You earned ${earned}.");
    }

    private void ShowDialogue(string message)
    {
        Debug.Log(message);

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.ShowMessage(message);
        }
    }

    public void ResetDailyDialogue()
    {
        hasTalkedToday = false;
    }
}