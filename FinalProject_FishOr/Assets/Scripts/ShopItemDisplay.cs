using UnityEngine;
using TMPro;

public class ShopItemDisplay : MonoBehaviour, IInteractable, IHoverable
{
    [Header("Item Settings")]
    public ItemData itemData;
    public int amount = 1;
    public int unlockDay = 1;
    public bool oneTimePurchase = false;

    [Header("Visual Settings")]
    public float hoverScaleMultiplier = 1.2f;
    public float scaleSpeed = 10f;

    [Header("UI")]
    public TextMeshProUGUI shopInfoText;

    private Vector3 originalScale;
    private bool isHovering = false;
    private bool hasPurchased = false;

    private void Start()
    {
        originalScale = transform.localScale;

        AutoFindShopInfoText();
    }

    private void Update()
    {
        Vector3 targetScale = isHovering ? originalScale * hoverScaleMultiplier : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public string GetInteractText()
    {
        if (!IsUnlocked())
        {
            return $"Locked until Day {unlockDay}";
        }

        if (oneTimePurchase && hasPurchased)
        {
            return "Already purchased";
        }

        if (itemData == null)
        {
            return "Item missing";
        }

        return $"Press E to buy {itemData.itemName} - ${itemData.buyPrice}";
    }

    public void Interact()
    {
        if (itemData == null)
        {
            Debug.LogWarning("ShopItemDisplay missing ItemData.");
            return;
        }

        if (!IsUnlocked())
        {
            ShowInfo($"Locked. Available on Day {unlockDay}.");
            return;
        }

        if (oneTimePurchase && hasPurchased)
        {
            ShowInfo($"{itemData.itemName} already purchased.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            ShowInfo("No Player found.");
            return;
        }

        InventorySystem inventory = player.GetComponent<InventorySystem>();

        if (inventory == null)
        {
            ShowInfo("Player has no InventorySystem.");
            return;
        }

        if (!GameManager.Instance.SpendMoney(itemData.buyPrice))
        {
            ShowInfo($"Not enough money. Need ${itemData.buyPrice}.");
            return;
        }

        if (itemData.itemType == ItemType.Upgrade)
        {
            inventory.ExpandSlots(2);
            hasPurchased = true;
            ShowInfo("Backpack expanded by 2 slots.");
            return;
        }

        bool added = inventory.AddItem(itemData, amount);

        if (!added)
        {
            GameManager.Instance.AddMoney(itemData.buyPrice);
            ShowInfo("Inventory is full.");
            return;
        }

        hasPurchased = true;

        ShowInfo($"Bought {itemData.itemName} x{amount}.");
    }

    public void OnLookEnter()
    {
        isHovering = true;
        ShowCurrentItemInfo();
    }

    public void OnLookStay()
    {
        ShowCurrentItemInfo();
    }

    public void OnLookExit()
    {
        isHovering = false;

        if (shopInfoText != null)
            shopInfoText.text = "";
    }

    private bool IsUnlocked()
    {
        if (GameManager.Instance == null)
            return false;

        return GameManager.Instance.currentDay >= unlockDay;
    }

    private void ShowCurrentItemInfo()
    {
        if (itemData == null)
        {
            ShowInfo("Missing item data.");
            return;
        }

        if (!IsUnlocked())
        {
            ShowInfo($"{itemData.itemName}\nLocked until Day {unlockDay}");
            return;
        }

        if (oneTimePurchase && hasPurchased)
        {
            ShowInfo($"{itemData.itemName}\nAlready purchased");
            return;
        }

        ShowInfo($"{itemData.itemName}\nPrice: ${itemData.buyPrice}\nAmount: {amount}");
    }

    private void ShowInfo(string message)
    {
        AutoFindShopInfoText();

        if (shopInfoText != null)
            shopInfoText.text = message;

        Debug.Log(message);
    }

    private void AutoFindShopInfoText()
    {
        if (shopInfoText != null)
            return;

        GameObject obj = GameObject.Find("ShopInfoText");

        if (obj != null)
            shopInfoText = obj.GetComponent<TextMeshProUGUI>();
    }
}