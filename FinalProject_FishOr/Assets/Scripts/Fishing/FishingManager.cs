using UnityEngine;
using TMPro;

public class FishingManager : MonoBehaviour
{
    public float timeRemaining;
    public TextMeshProUGUI timerText;

    private bool isRunning = true;

    void Start()
    {
        timeRemaining = GameManager.Instance.fishingTimeLimit;
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0) timeRemaining = 0;

        int min = Mathf.FloorToInt(timeRemaining / 60f);
        int sec = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"Time: {min:00}:{sec:00}";

        if (timeRemaining <= 0)
        {
            EndFishingDay();
        }
    }

    public void EndFishingDay()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            MysteryBoxEffect effect = player.GetComponent<MysteryBoxEffect>();

            if (effect != null)
                effect.StopEffect();
        }
        isRunning = false;

        if (GameManager.Instance.IsOrderComplete())
        {
            Debug.Log("Order completed. Return to the store and proceed to the next day.");
            GameManager.Instance.AdvanceToNextDay();
        }
        else
        {
            Debug.Log("Order failed, requirements not met");
            GameManager.Instance.FailDayAndBackToMenu();
        }
    }
}