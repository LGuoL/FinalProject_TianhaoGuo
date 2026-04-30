using UnityEngine;

public class DeliveryCrate : MonoBehaviour, IInteractable
{
    public BucketSystem playerBucket;
    public FishingManager fishingManager;

    public string GetInteractText()
    {
        return $"Press E to submit fish ({GameManager.Instance.currentOrderSubmitted}/{GameManager.Instance.currentOrderRequired})";
    }

    public void Interact()
    {
        if (playerBucket == null)
        {
            Debug.LogWarning("DeliveryCrate: playerBucket is missing.");
            return;
        }

        if (GameManager.Instance.IsOrderComplete())
        {
            Debug.Log("Order complete. Returning to shop.");
            GameManager.Instance.AdvanceToNextDay();
            return;
        }

        int remainingNeed = GameManager.Instance.currentOrderRequired - GameManager.Instance.currentOrderSubmitted;

        int submitted = playerBucket.RemoveFish(remainingNeed);

        if (submitted <= 0)
        {
            Debug.Log("No fish in bucket.");
            return;
        }

        GameManager.Instance.SubmitFish(submitted);

        Debug.Log($"Submitted {submitted} fish. Progress: {GameManager.Instance.currentOrderSubmitted}/{GameManager.Instance.currentOrderRequired}");

        if (GameManager.Instance.IsOrderComplete())
        {
            Debug.Log("Order complete! Press E again to return to shop.");
        }
    }
}