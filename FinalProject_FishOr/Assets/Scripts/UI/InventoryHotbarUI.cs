using System.Collections.Generic;
using UnityEngine;

public class InventoryHotbarUI : MonoBehaviour
{
    public InventorySystem inventory;
    public InventorySlotUI slotPrefab;
    public Transform slotParent;

    [Header("Settings")]
    public int visibleSlots = 8;
    public int selectedIndex = 0;

    private readonly List<InventorySlotUI> spawnedSlots = new List<InventorySlotUI>();

    private void Start()
    {
        RebuildSlots();
    }

    private void Update()
    {
        if (inventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                inventory = player.GetComponent<InventorySystem>();
        }

        HandleNumberInput();
        Refresh();
    }

    private void HandleNumberInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) selectedIndex = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) selectedIndex = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) selectedIndex = 7;
    }

    public void RebuildSlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        spawnedSlots.Clear();

        for (int i = 0; i < visibleSlots; i++)
        {
            InventorySlotUI slot = Instantiate(slotPrefab, slotParent);
            spawnedSlots.Add(slot);
        }
    }

    public void Refresh()
    {
        if (inventory == null)
            return;

        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            if (i < inventory.slots.Count)
            {
                spawnedSlots[i].SetSlot(inventory.slots[i], i == selectedIndex);
            }
            else
            {
                spawnedSlots[i].ClearSlot();
            }
        }
    }
}