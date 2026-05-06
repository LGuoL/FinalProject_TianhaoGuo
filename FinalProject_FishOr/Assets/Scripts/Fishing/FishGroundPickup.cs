using UnityEngine;

public class FishGroundPickup : MonoBehaviour
{
    public int fishAmount = 1;
    public FishData fishData;

    public int GetSellPrice()
    {
        if (fishData == null)
            return 5;

        return fishData.sellPrice;
    }

    public string GetFishName()
    {
        if (fishData == null)
            return "Fish";

        return fishData.fishName;
    }
}