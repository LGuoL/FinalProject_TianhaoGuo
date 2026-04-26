using UnityEngine;

public enum ItemType
{
    Note,
    Rod,
    Bait,
    Bucket,
    Fish,
    Weapon,
    Ammo,
    Special,
    Upgrade
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "FishingGame/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public int buyPrice;
    public int sellPrice;
    public bool stackable = true;
}