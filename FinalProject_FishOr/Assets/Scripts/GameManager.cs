using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progress")]
    public int currentDay = 1;
    public int money = 0;

    [Header("Order")]
    public int currentOrderRequired = 8;
    public int currentOrderSubmitted = 0;

    [Header("Fishing Day Settings")]
    public float fishingTimeLimit = 180f; // 3分钟

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GenerateOrderForDay();
    }

    public void GenerateOrderForDay()
    {
        currentOrderSubmitted = 0;

        // 订单只要求“总鱼数量”
        // 第三天后明显提高难度
        switch (currentDay)
        {
            case 1: currentOrderRequired = 8; break;
            case 2: currentOrderRequired = 12; break;
            case 3: currentOrderRequired = 20; break;
            case 4: currentOrderRequired = 45; break;
            case 5: currentOrderRequired = 80; break;
            default: currentOrderRequired = 120 + (currentDay - 6) * 30; break;
        }
    }

    public bool IsOrderComplete()
    {
        return currentOrderSubmitted >= currentOrderRequired;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;
        money -= amount;
        return true;
    }

    public void SubmitFish(int amount)
    {
        currentOrderSubmitted += amount;
        if (currentOrderSubmitted > currentOrderRequired)
            currentOrderSubmitted = currentOrderRequired;
    }

    public void GoToFishingScene()
    {
        SceneManager.LoadScene("FishingArea");
    }

    public void ReturnToShopScene()
    {
        SceneManager.LoadScene("ShopArea");
    }

    public void AdvanceToNextDay()
    {
        currentDay++;
        GenerateOrderForDay();
        ReturnToShopScene();
    }

    public void FailDayAndBackToMenu()
    {
        // 你也可以改成 Game Over UI 后再回主菜单
        SceneManager.LoadScene("ShopArea");
        currentDay = 1;
        money = 0;
        GenerateOrderForDay();
    }
}