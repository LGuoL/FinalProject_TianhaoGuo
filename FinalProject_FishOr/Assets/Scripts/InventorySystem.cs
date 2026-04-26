using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;
}

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 4;
    public List<InventorySlot> slots = new List<InventorySlot>();

    [Header("Initial Items")]
    public ItemData noteItem;
    public ItemData basicRodItem;
    public ItemData basicBaitItem;
    public ItemData bucketItem;

    private void Start()
    {
        if (slots.Count == 0)
        {
            AddItem(noteItem, 1);
            AddItem(basicRodItem, 1);
            AddItem(basicBaitItem, 10);
            AddItem(bucketItem, 1);
        }
    }

    public bool AddItem(ItemData item, int amount)
    {
        if (item == null) return false;

        if (item.stackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item)
                {
                    slot.count += amount;
                    return true;
                }
            }
        }

        if (slots.Count >= maxSlots) return false;

        InventorySlot newSlot = new InventorySlot
        {
            item = item,
            count = amount
        };

        slots.Add(newSlot);
        return true;
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                if (slot.count >= amount)
                {
                    slot.count -= amount;

                    if (slot.count <= 0)
                    {
                        slots.Remove(slot);
                    }
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public bool HasItem(ItemData item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.count >= amount)
                return true;
        }
        return false;
    }

    public void ExpandSlots(int amount)
    {
        maxSlots += amount;
    }
}