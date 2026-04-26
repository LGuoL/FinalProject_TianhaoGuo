using UnityEngine;

public class FishingStartSign : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Press E to start today's fishing activity.";
    }

    public void Interact()
    {
        GameManager.Instance.GoToFishingScene();
    }
}