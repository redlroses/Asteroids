using System;
using UnityEngine;

public class OldWeapon : MonoBehaviour
{
    public static event EventHandler<bool> OnMaxWeaponUpgrade;
    
    public enum WeaponType { Cannon, Laser, Missile }

    public float GetShotsCoolDown => ShotsCoolDown;
    public int GetDamage => Damage;
    public float GetUsedTime => UsedTime;
    public WeaponType GetWeaponType => Type;

    protected int Damage = 0;
    protected float ShotsCoolDown = 0f;
    protected float UsedTime = 0f;
    protected WeaponType Type;
    protected Bonus.RarityType _weaponRarity = Bonus.RarityType.Common;

    public void SetParameters(int damage, float shotsCd, float usedTime)
    {
        Damage = damage;
        ShotsCoolDown = shotsCd;
        UsedTime = usedTime;
    }

    public virtual void SetParameters(int damage)
    {
        Damage = damage;
    }
    
    public virtual void SetParameters(float shotsCd)
    {
        ShotsCoolDown = shotsCd;
    }

    public virtual int GetDefaultDamage()
    {
        return 0;
    }

    public virtual float GetDefaultCd()
    {
        return 0;
    }

    public virtual void UpgradeRarity()
    {
        
    }

    protected bool IsUpgradeable()
    {
        return _weaponRarity != Bonus.RarityType.Legend;
    }

    protected void MaxUpgrade()
    {
        OnMaxWeaponUpgrade?.Invoke(this, false);
    }
}
