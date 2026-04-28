using UnityEngine;

public class BucketSystem : MonoBehaviour
{
    public int currentFishCount = 0;

    public void AddFish(int amount)
    {
        currentFishCount += amount;
    }

    public bool HasFish()
    {
        return currentFishCount > 0;
    }

    public int RemoveFish(int amount)
    {
        int removed = Mathf.Min(amount, currentFishCount);
        currentFishCount -= removed;
        return removed;
    }

    public int RemoveAllFish()
    {
        int total = currentFishCount;
        currentFishCount = 0;
        return total;
    }
}