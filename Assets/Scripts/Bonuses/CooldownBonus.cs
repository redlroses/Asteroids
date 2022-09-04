using UnityEngine;

public class CooldownBonus : Bonus
{
    [SerializeField, Range(0f, 1f)] private float _cooldownBonus;

    protected override void TryApply(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shooter shooter))
        {
            var weapon = shooter.Weapon;
            float cooldownAmount = _cooldownBonus * weapon.DefaultCooldown * _bonusScaleFactor;
            weapon.DecreaseCooldown(cooldownAmount);
        }
    }
}