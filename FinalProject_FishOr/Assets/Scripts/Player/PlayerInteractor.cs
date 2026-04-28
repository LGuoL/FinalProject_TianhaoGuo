using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 4f;
    public TextMeshProUGUI interactText;

    private IInteractable currentInteractable;

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractable()
    {
        currentInteractable = null;

        if (interactText != null)
        {
            interactText.text = "";
        }

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();

            if (playerCamera == null)
            {
                return;
            }
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                if (interactText != null)
                {
                    interactText.text = interactable.GetInteractText();
                }
            }
        }
    }
}