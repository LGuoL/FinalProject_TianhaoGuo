using UnityEngine;

public class FishingGearTester : MonoBehaviour
{
    public FishingRodController rodController;

    public RodData basicRod;
    public RodData advancedRod;

    public BaitData basicBait;
    public BaitData betterBait;
    public BaitData bestBait;

    void Update()
    {
        if (rodController == null)
            return;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            rodController.SetRod(basicRod);
            Debug.Log("Switched to Basic Rod");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            rodController.SetRod(advancedRod);
            Debug.Log("Switched to Advanced Rod");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            rodController.SetBait(basicBait);
            Debug.Log("Switched to Basic Bait");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            rodController.SetBait(betterBait);
            Debug.Log("Switched to Better Bait");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            rodController.SetBait(bestBait);
            Debug.Log("Switched to Best Bait");
        }
    }
}