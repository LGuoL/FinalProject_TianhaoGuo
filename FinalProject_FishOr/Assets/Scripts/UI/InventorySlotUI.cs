using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;
    public GameObject selectionBorder;

    public void SetSlot(InventorySlot slot, bool selected)
    {
        if (slot == null || slot.item == null)
        {
            ClearSlot();
            return;
        }

        if (iconImage != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = slot.item.icon;
        }

        if (countText != null)
        {
            if (slot.count > 1)
                countText.text = slot.count.ToString();
            else
                countText.text = "";
        }

        if (selectionBorder != null)
            selectionBorder.SetActive(selected);
    }

    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (countText != null)
            countText.text = "";

        if (selectionBorder != null)
            selectionBorder.SetActive(false);
    }
}