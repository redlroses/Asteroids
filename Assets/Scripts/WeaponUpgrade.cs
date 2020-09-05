using System;
using UnityEngine;

public class WeaponUpgrade : SpawningObjects
{
    public static event EventHandler<bool> OnWeaponUpgradeSpawns;
    
    private void Start()
    {
        OnWeaponUpgradeSpawns?.Invoke(this, false);
    }

    protected override void ApplyBonus(Player player)
    {
        player.Weapon.LevelUp();
    }
}

