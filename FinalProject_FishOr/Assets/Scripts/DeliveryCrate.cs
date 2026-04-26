using UnityEngine;

public class DeliveryCrate : MonoBehaviour, IInteractable
{
    public BucketSystem playerBucket;
    public FishingManager fishingManager;

    public string GetInteractText()
    {
        return $"Submit the fish by pressing E ({GameManager.Instance.currentOrderSubmitted}/{GameManager.Instance.currentOrderRequired})";
    }

    public void Interact()
    {
        if (playerBucket == null) return;

        int remainingNeed = GameManager.Instance.currentOrderRequired - GameManager.Instance.currentOrderSubmitted;
        if (remainingNeed <= 0)
        {
            Debug.Log("订单已完成，再按一次可等待结算/时间结束");
            return;
        }

        int submitted = playerBucket.RemoveFish(remainingNeed);
        GameManager.Instance.SubmitFish(submitted);

        Debug.Log($"已提交 {submitted} 条鱼，当前进度：{GameManager.Instance.currentOrderSubmitted}/{GameManager.Instance.currentOrderRequired}");

        if (GameManager.Instance.IsOrderComplete())
        {
            Debug.Log("订单完成！你可以继续捞鱼，也可以等待时间结束。");
        }
    }
}