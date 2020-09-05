using UnityEngine;

public class CooldownBonus : Bonus
{
    [SerializeField, Tooltip("In percents")]
    private int _defaultCooldownIncrease;

    protected override void ApplyBonus(Player player)
    {
        var weapon = player.Weapon;
        weapon.Cooldown -= CalculateBonus(weapon.DefaultCooldown, _defaultCooldownIncrease, _rarity, _bonusScaleFactor);
        Debug.Log($"Скорострельность уменьшена до {weapon.Cooldown}");
    }
}