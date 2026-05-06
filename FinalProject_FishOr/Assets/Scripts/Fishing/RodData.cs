using UnityEngine;

[CreateAssetMenu(fileName = "NewRodData", menuName = "FishingGame/Rod Data")]
public class RodData : ScriptableObject
{
    public string rodName;

    [Header("Fishing Speed")]
    public float biteTimeMultiplier = 1f;

    [Header("Future QTE")]
    public float qteDifficultyMultiplier = 1f;
}