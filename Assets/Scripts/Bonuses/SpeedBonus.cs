using UnityEngine;

public class SpeedBonus : Bonus
{
    [SerializeField, Range(0f, 1f)] private float _speedBonus;

    protected override void TryApply(Collider2D collision)
    {
        if (collision.TryGetComponent(out Mover mover))
        {
            float speedAmount = _speedBonus * mover.DefaultMoveSpeed * _bonusScaleFactor;
            mover.IncreaseSpeed(speedAmount);
        }
    }
}