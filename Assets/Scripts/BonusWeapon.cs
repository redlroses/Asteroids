using UnityEngine;

public class BonusWeapon : Bonus
{ 
    public OldWeapon.WeaponType WeaponType { get; set; } = OldWeapon.WeaponType.Cannon;
    
    [SerializeField] private float _defaultUsedTime = 10f; //Время использования оружия по умолчанию

    protected override void ApplyBonus(Player player)
    {
        // Weapon weapon;
        //
        // //Кладём в переменную weapon оружие нужного типа
        // switch (WeaponType)
        // {
        //     case Weapon.WeaponType.Cannon:
        //         weapon = collision.gameObject.GetComponent<Cannon>();
        //         break;
        //
        //     case Weapon.WeaponType.Laser:
        //         weapon = collision.gameObject.GetComponent<Laser>();
        //         break;
        //     default:
        //         Debug.LogError("Error WeaponType in BonusWeapon");
        //         weapon = null;
        //         break;
        // }
        //
        // //Достаём из параметров оружия значения по умолчанию
        // int defaultDamage = weapon.GetDefaultDamage();
        // float defaultCoolDown = weapon.GetDefaultCd();
        //
        // //Скейлим основные параметры в зависимости от редкости оружия
        // int scaledDamage = Mathf.RoundToInt(defaultDamage * ((int)_rarity + 1) * _bonusScaleFactor);
        // float scaledCd = defaultCoolDown / ((int)_rarity + 1) / _bonusScaleFactor;
        // float scaledUsedTime = _defaultUsedTime * ((int)_rarity + 1) * _bonusScaleFactor;
        //
        // //Устанавливаем эти параметры
        // weapon.SetParameters(scaledDamage, scaledCd, scaledUsedTime);
        //
        // //Устанавливаем данное оружие как основное
        // weaponController.SetCurrentWeapon(weapon, scaledUsedTime);
        //
        // Destroy(gameObject);
    }
}
