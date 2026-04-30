using UnityEngine;
using TMPro;

public enum EquipmentType
{
    None,
    Rod,
    Bucket
}

public class PlayerEquipment : MonoBehaviour
{
    public EquipmentType currentEquipment = EquipmentType.Rod;

    [Header("References")]
    public FishingRodController fishingRodController;
    public BucketCollector bucketCollector;

    [Header("UI")]
    public TextMeshProUGUI equipmentText;

    private void Start()
    {
        RefreshUI();
    }

    private void Update()
    {
        HandleSwitchInput();
        HandleUseInput();
    }

    private void HandleSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(EquipmentType.Rod);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(EquipmentType.Bucket);
        }
    }

    private void HandleUseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseCurrentEquipment();
        }
    }

    public void Equip(EquipmentType type)
    {
        currentEquipment = type;
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
        }
    }

    private void RefreshUI()
    {
        if (equipmentText != null)
        {
            equipmentText.text = "Equipped: " + currentEquipment;
        }
    }
}