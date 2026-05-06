using UnityEngine;

[CreateAssetMenu(fileName = "NewFishData", menuName = "FishingGame/Fish Data")]
public class FishData : ScriptableObject
{
    public string fishName;
    public GameObject fishPrefab;
    public int sellPrice = 5;

    [Header("Chance")]
    public float baseWeight = 1f;
}