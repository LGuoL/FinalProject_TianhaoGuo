using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentDay = 1;
    public int money = 0;

    public int currentOrderRequiredFish = 0;
    public int currentSubmittedFish = 0;

    public bool day3ShopDialogueTriggered = false;

    public static GameManager Instance;

    public void StartNewDay()
    {
        currentSubmittedFish = 0;
        currentOrderRequiredFish = GetRequiredFishByDay(currentDay);
    }

    public int GetRequiredFishByDay(int day)
    {
        switch (day)
        {
            case 1: return 8;
            case 2: return 12;
            case 3: return 20;
            case 4: return 45;
            case 5: return 80;
            default: return 120 + (day - 6) * 30;
        }
    }
    
}
