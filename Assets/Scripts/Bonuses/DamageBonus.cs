using UnityEngine;

public class DamageBonus : Bonus
{
    [SerializeField, Range(0f, 1f)] private float _damageBonus;

    protected override void TryApply(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shooter shooter))
        {
            var weapon = shooter.Weapon;
            int damageAmount = Mathf.RoundToInt(_damageBonus * weapon.DefaultDamage * _bonusScaleFactor);
            weapon.IncreaseDamage(damageAmount);
        }
    }
}