using UnityEngine;

public class FishSeller : MonoBehaviour, IInteractable
{
    public int pricePerFish = 10;
    public BucketSystem playerBucket;

    public string GetInteractText()
    {
        return "Sell all the fish in the bucket by pressing the \"E\" button.";
    }

    public void Interact()
    {
        if (playerBucket == null) return;

        int totalFish = playerBucket.RemoveAllFish();
        if (totalFish > 0)
        {
            int earned = totalFish * pricePerFish;
            GameManager.Instance.AddMoney(earned);
            Debug.Log($"sale {totalFish} Fishes, Gain {earned} Coins");
        }
        else
        {
            Debug.Log("There are no fish in the bucket to sell.");
        }
    }
}