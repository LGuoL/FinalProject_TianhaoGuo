using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 6f;
    public TextMeshProUGUI interactText;

    private IInteractable currentInteractable;

    void Start()
    {
        AutoReconnect();
    }

    void Update()
    {
        if (playerCamera == null || interactText == null)
        {
            AutoReconnect();
        }

        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void AutoReconnect()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        if (interactText == null)
        {
            GameObject textObj = GameObject.Find("InteractText");

            if (textObj != null)
            {
                interactText = textObj.GetComponent<TextMeshProUGUI>();
            }
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
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable == null)
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();
            }

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