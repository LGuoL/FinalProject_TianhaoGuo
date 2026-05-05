using UnityEngine;

public class NoteUI : MonoBehaviour
{
    public static bool hasShownOnce = false;

    public GameObject notePanel;
    public bool showOnStart = true;
    public KeyCode closeKey = KeyCode.E;

    private bool isOpen = false;

    private void Start()
    {
        if (showOnStart && !hasShownOnce)
        {
            hasShownOnce = true;
            OpenNote();
        }
        else
        {
            CloseNote();
        }
    }

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(closeKey))
        {
            CloseNote();
        }
    }

    public void OpenNote()
    {
        isOpen = true;

        if (notePanel != null)
            notePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void CloseNote()
    {
        isOpen = false;

        if (notePanel != null)
            notePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}