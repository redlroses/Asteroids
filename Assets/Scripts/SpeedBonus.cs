using UnityEngine;

public class SpeedBonus : Bonus
{
    [Tooltip("In percents")] [SerializeField]
    private int _defaultSpeedIncrease;

    protected override void ApplyBonus(Player player)
    {
        var mover = player.Mover;
        mover.MoveSpeed += CalculateBonus(mover.DefaultMoveSpeed, _defaultSpeedIncrease, _rarity, _bonusScaleFactor);
        Debug.Log($"Скорость увеличена до {mover.MoveSpeed}");
    }
}