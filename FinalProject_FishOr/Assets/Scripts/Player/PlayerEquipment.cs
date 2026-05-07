using UnityEngine;
using TMPro;

public enum EquipmentType
{
    None,
    Rod,
    Bucket,
    SMG,
    Grenade,
    RPG
}

public class PlayerEquipment : MonoBehaviour
{
    public EquipmentType currentEquipment = EquipmentType.Rod;

    [Header("References")]
    public FishingRodController fishingRodController;
    public BucketCollector bucketCollector;
    public WeaponFishingController weaponFishingController;
    public InventorySystem inventory;
    public PlayerWeaponVisuals weaponVisuals;

    [Header("Required Items")]
    public ItemData smgItem;
    public ItemData grenadeItem;
    public ItemData rpgItem;

    [Header("UI")]
    public TextMeshProUGUI equipmentText;

    private void Start()
    {
        AutoFindReferences();
        Equip(EquipmentType.Rod);
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        HandleSwitchInput();
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
    }

    private void HandleSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Equip(EquipmentType.Rod);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Equip(EquipmentType.Bucket);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            TryEquipWeapon(EquipmentType.SMG);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            TryEquipWeapon(EquipmentType.Grenade);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            TryEquipWeapon(EquipmentType.RPG);
    }

    private void TryEquipWeapon(EquipmentType type)
    {
        AutoFindReferences();

        if (inventory == null)
        {
            Debug.LogWarning("No InventorySystem found.");
            return;
        }

        if (type == EquipmentType.SMG)
        {
            if (!inventory.HasItem(smgItem, 1))
            {
                Debug.Log("You have not bought SMG yet.");
                return;
            }
        }

        if (type == EquipmentType.Grenade)
        {
            if (!inventory.HasItem(grenadeItem, 1))
            {
                Debug.Log("You have no grenade.");
                return;
            }
        }

        if (type == EquipmentType.RPG)
        {
            if (!inventory.HasItem(rpgItem, 1))
            {
                Debug.Log("You have not bought RPG yet.");
                return;
            }
        }

        Equip(type);
    }

    private void HandleUseInput()
    {
        if (Input.GetMouseButtonDown(0))
            UseCurrentEquipment();
    }

    public void Equip(EquipmentType type)
    {
        currentEquipment = type;

        if (weaponVisuals != null)
        {
            weaponVisuals.ShowEquipment(type);
        }

        RefreshUI();

        Debug.Log("Equipped: " + currentEquipment);
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
        }
    }

    public void RefreshUI()
    {
        if (equipmentText != null)
            equipmentText.text = "Equipped: " + currentEquipment;
    }
}