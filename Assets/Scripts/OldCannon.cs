using System;
using System.Collections.Generic;
using UnityEngine;

public class OldCannon : OldWeapon, IShootable
{
    [SerializeField] private WeaponType _weaponType = WeaponType.Cannon;
    [SerializeField] private float _defaultShotsCoolDown;
    [SerializeField] private int _defaultDamage;
    [SerializeField] private GameObject _bulletPrefub;
    [SerializeField] private Transform _projectilesParent;
    [SerializeField] private Transform[] _levelsSpawnPoints;
    
    private List<Transform> _bulletSpawnPoints = new List<Transform>();

    private void Start()
    {
        ShotsCoolDown = _defaultShotsCoolDown;
        Damage = _defaultDamage;
        _bulletPrefub.GetComponent<LaserBullet>().SetDamage(Damage);
        SetSpawnPoints();
    }

    public void Shoot()
    {
        foreach (var item in _bulletSpawnPoints)
        {
            Instantiate(_bulletPrefub, item.position, item.rotation, _projectilesParent);
            Debug.Log($"Cannon Shoot: {Damage} damage, {ShotsCoolDown} cool down");
        }
    }

    public override int GetDefaultDamage()
    {
        return _defaultDamage;
    }

    public override float GetDefaultCd()
    {
        return _defaultShotsCoolDown;
    }
    
    public override void SetParameters(int damage)
    {
        _bulletPrefub.GetComponent<LaserBullet>().SetDamage(damage);
        Damage = damage;
    }

    public override void UpgradeRarity()
    {
        if (IsUpgradeable() == false)
        {
            MaxUpgrade();
            return;
        }
        
        _weaponRarity = (Bonus.RarityType) ((int)_weaponRarity + 1);
        SetSpawnPoints();
    }

    private void SetSpawnPoints()
    {
        _bulletSpawnPoints.Clear();
        foreach (Transform item in _levelsSpawnPoints[(int)_weaponRarity])
        {
            _bulletSpawnPoints.Add(item);
        }
    }
}
