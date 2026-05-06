using UnityEngine;

[CreateAssetMenu(fileName = "NewBaitData", menuName = "FishingGame/Bait Data")]
public class BaitData : ScriptableObject
{
    public string baitName;
    public ItemData baitItem;

    [Header("Fishing Speed")]
    public float biteTimeMultiplier = 1f;

    [Header("Rare Fish Bonus")]
    public float rareFishBonus = 0f;
}