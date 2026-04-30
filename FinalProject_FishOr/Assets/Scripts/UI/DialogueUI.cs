using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public TextMeshProUGUI dialogueText;
    public float showTime = 3f;

    private Coroutine hideRoutine;

    private void Awake()
    {
        Instance = this;

        if (dialogueText != null)
            dialogueText.text = "";
    }

    public void ShowMessage(string message)
    {
        if (dialogueText == null) return;

        dialogueText.text = message;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showTime);

        if (dialogueText != null)
            dialogueText.text = "";
    }
}