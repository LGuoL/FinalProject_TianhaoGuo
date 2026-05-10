using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EquipmentType
{
    Rod,
    Bucket,
    SMG,
    Grenade,
    RPG,
    MysteryBox
}

public class PlayerEquipment : MonoBehaviour
{
    [Header("Current Equipment")]
    public EquipmentType currentEquipment = EquipmentType.Rod;

    [Header("References")]
    public FishingRodController fishingRodController;
    public BucketCollector bucketCollector;
    public WeaponFishingController weaponFishingController;
    public InventorySystem inventory;
    public PlayerWeaponVisuals weaponVisuals;
    public MysteryBoxEffect mysteryBoxEffect;

    [Header("Required Items")]
    public ItemData rodItem;
    public ItemData bucketItem;
    public ItemData smgItem;
    public ItemData grenadeItem;
    public ItemData rpgItem;
    public ItemData mysteryBoxItem;

    [Header("UI")]
    public TextMeshProUGUI equipmentText;

    private readonly List<EquipmentType> availableEquipments = new List<EquipmentType>();
    private int currentIndex = 0;

    private void Start()
    {
        AutoFindReferences();
        RebuildAvailableEquipments();
        Equip(EquipmentType.Rod);
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        AutoFindReferences();
        RebuildAvailableEquipments();
        HandleScrollInput();
        HandleUseInput();
    }

    private void AutoFindReferences()
    {
        if (inventory == null)
            inventory = GetComponent<InventorySystem>();

        if (fishingRodController == null)
            fishingRodController = GetComponent<FishingRodController>();

        if (bucketCollector == null)
            bucketCollector = GetComponent<BucketCollector>();

        if (weaponFishingController == null)
            weaponFishingController = GetComponent<WeaponFishingController>();

        if (weaponVisuals == null)
            weaponVisuals = GetComponent<PlayerWeaponVisuals>();
        
        if (mysteryBoxEffect == null)
            mysteryBoxEffect = GetComponent<MysteryBoxEffect>();

        if (equipmentText == null)
        {
            GameObject obj = GameObject.Find("EquippedText");
            if (obj != null)
                equipmentText = obj.GetComponent<TextMeshProUGUI>();
        }
    }

    private void RebuildAvailableEquipments()
    {
        availableEquipments.Clear();

        if (HasRequiredItem(EquipmentType.Rod))
            availableEquipments.Add(EquipmentType.Rod);

        if (HasRequiredItem(EquipmentType.Bucket))
            availableEquipments.Add(EquipmentType.Bucket);

        if (HasRequiredItem(EquipmentType.SMG))
            availableEquipments.Add(EquipmentType.SMG);

        if (HasRequiredItem(EquipmentType.Grenade))
            availableEquipments.Add(EquipmentType.Grenade);

        if (HasRequiredItem(EquipmentType.RPG))
            availableEquipments.Add(EquipmentType.RPG);

        if (HasRequiredItem(EquipmentType.MysteryBox))
            availableEquipments.Add(EquipmentType.MysteryBox);

        if (availableEquipments.Count == 0)
        {
            availableEquipments.Add(EquipmentType.Rod);
        }

        if (!availableEquipments.Contains(currentEquipment))
        {
            Equip(availableEquipments[0]);
        }
    }

    private bool HasRequiredItem(EquipmentType type)
    {
        if (inventory == null)
            return false;

        switch (type)
        {
            case EquipmentType.Rod:
                return rodItem == null || inventory.HasItem(rodItem, 1);

            case EquipmentType.Bucket:
                return bucketItem == null || inventory.HasItem(bucketItem, 1);

            case EquipmentType.SMG:
                return smgItem != null && inventory.HasItem(smgItem, 1);

            case EquipmentType.Grenade:
                return grenadeItem != null && inventory.HasItem(grenadeItem, 1);

            case EquipmentType.RPG:
                return rpgItem != null && inventory.HasItem(rpgItem, 1);

            case EquipmentType.MysteryBox:
                return mysteryBoxItem != null && inventory.HasItem(mysteryBoxItem, 1);
        }

        return false;
    }

    private void HandleScrollInput()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0f)
        {
            SwitchEquipment(-1);
        }
        else if (scroll < 0f)
        {
            SwitchEquipment(1);
        }
    }

    private void SwitchEquipment(int direction)
    {
        if (availableEquipments.Count == 0)
            return;

        currentIndex = availableEquipments.IndexOf(currentEquipment);

        if (currentIndex < 0)
            currentIndex = 0;

        currentIndex += direction;

        if (currentIndex < 0)
            currentIndex = availableEquipments.Count - 1;

        if (currentIndex >= availableEquipments.Count)
            currentIndex = 0;

        Equip(availableEquipments[currentIndex]);
    }

    public void Equip(EquipmentType type)
    {
        if (!HasRequiredItem(type))
        {
            Debug.Log("Cannot equip: " + type);
            return;
        }

        currentEquipment = type;
        currentIndex = availableEquipments.IndexOf(type);

        if (weaponVisuals != null)
        {
            weaponVisuals.ShowEquipment(type);
        }

        RefreshUI();

        Debug.Log("Equipped: " + currentEquipment);
    }

    private void HandleUseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseCurrentEquipment();
        }
    }

    private void UseCurrentEquipment()
    {
        switch (currentEquipment)
        {
            case EquipmentType.Rod:
                if (fishingRodController != null)
                    fishingRodController.TryFish();
                break;

            case EquipmentType.Bucket:
                if (bucketCollector != null)
                    bucketCollector.TryCollectFish();
                break;

            case EquipmentType.SMG:
                if (weaponFishingController != null)
                    weaponFishingController.UseSMG();
                break;

            case EquipmentType.Grenade:
                if (weaponFishingController != null)
                    weaponFishingController.ThrowGrenade();
                break;

            case EquipmentType.RPG:
                if (weaponFishingController != null)
                    weaponFishingController.FireRPG();
                break;

            case EquipmentType.MysteryBox:
                if (mysteryBoxEffect != null)
                    mysteryBoxEffect.UseMysteryBox();
                break;


        }
    }

    public void RefreshUI()
    {
        if (equipmentText != null)
        {
            equipmentText.text = $"Equipped: {currentEquipment}";
        }
    }
}