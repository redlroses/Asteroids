using UnityEngine;

public class DamageBonus : Bonus
{
    [Tooltip("In percents"), SerializeField]
    private int _defaultDamageIncrease;

    protected override void ApplyBonus(Player player)
    {
        var weapon = player.Weapon;
        weapon.Damage += CalculateBonus(weapon.DefaultDamage, _defaultDamageIncrease, _rarity, _bonusScaleFactor);
        Debug.Log($"Урон увеличен до {weapon.Damage}");
    }
}