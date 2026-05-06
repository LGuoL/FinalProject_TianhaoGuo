using UnityEngine;

public class BucketSystem : MonoBehaviour
{
    public int currentFishCount = 0;
    public int currentFishValue = 0;

    public void AddFish(int amount)
    {
        currentFishCount += amount;
        currentFishValue += amount * 10;
    }

    public void AddFish(FishData fishData, int amount)
    {
        currentFishCount += amount;

        if (fishData != null)
            currentFishValue += fishData.sellPrice * amount;
        else
            currentFishValue += 5 * amount;
    }

    public bool HasFish()
    {
        return currentFishCount > 0;
    }

    public int RemoveFish(int amount)
    {
        int removed = Mathf.Min(amount, currentFishCount);

        if (currentFishCount > 0)
        {
            float valuePerFish = (float)currentFishValue / currentFishCount;
            currentFishValue -= Mathf.RoundToInt(valuePerFish * removed);
        }

        currentFishCount -= removed;

        if (currentFishCount <= 0)
        {
            currentFishCount = 0;
            currentFishValue = 0;
        }

        return removed;
    }

    public int RemoveAllFish()
    {
        int total = currentFishCount;
        currentFishCount = 0;
        currentFishValue = 0;
        return total;
    }

    public int RemoveAllFishValue()
    {
        int value = currentFishValue;
        currentFishCount = 0;
        currentFishValue = 0;
        return value;
    }
}