using UnityEngine;

public abstract class Bonus : SpawningObjects
{
    public enum RarityType
    {
        Common,
        Rare,
        Legend
    }

    public RarityType Rarity
    {
        get => _rarity;
        set => _rarity = value;
    }

    [SerializeField] protected float _bonusScaleFactor;
    [SerializeField] protected RarityType _rarity;

    protected static float CalculateBonus(float defaultValue, float increasePercent, RarityType rarity,
        float scaleFactor)
    {
        return defaultValue * increasePercent / 100f * ((int) rarity + 1) * scaleFactor;
    }

    protected static int CalculateBonus(int defaultValue, float increasePercent, RarityType rarity,
        float scaleFactor)
    {
        return Mathf.RoundToInt(defaultValue * increasePercent / 100f * ((int) rarity + 1) * scaleFactor);
    }
}