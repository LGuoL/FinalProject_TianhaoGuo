using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "按 E 测试交互";
    }

    public void Interact()
    {
        Debug.Log("你按下了 E，并成功交互！");
    }
}