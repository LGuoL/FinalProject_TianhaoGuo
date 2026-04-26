using UnityEngine;

public class MerchantNPC : MonoBehaviour, IInteractable
{
    private bool hasTalkedToday = false;

    public string GetInteractText()
    {
        if (GameManager.Instance.currentDay >= 3 && !hasTalkedToday)
            return "Press E to talk to the merchant (new goods available)";

        return "Press E to talk to the merchant / accept the order.";
    }

    public void Interact()
    {
        if (GameManager.Instance.currentDay >= 3 && !hasTalkedToday)
        {
            hasTalkedToday = true;
            Debug.Log("Merchant: Starting from the third day, new items will be put on display. Don't just focus on normal fishing for now.");
        }
        else
        {
            Debug.Log($"Today's order: A total of submissions are required {GameManager.Instance.currentOrderRequired} Fishes¡£");
        }
    }

    public void ResetDailyDialogue()
    {
        hasTalkedToday = false;
    }
}