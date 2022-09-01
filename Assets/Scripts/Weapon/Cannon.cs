using System;
using UnityEngine;

public class Cannon : Weapon
{
    [SerializeField] private LaserBullet _bullet;
    [SerializeField] private Transform _projectilesContainer;
    [SerializeField] private Transform[] _levelShootPoints;

    public override void Shoot()
    {
        foreach (var point in ShootPoints)
        {
            Instantiate(_bullet, point.position, point.rotation, _projectilesContainer);
        }
    }

    public override void Initialize(Transform projectilesContainer)
    {
        InitializeParameters();
        OnDamageChanged += SetProjectileDamage; //TODO: сделать прямую зависимость без события
        SetProjectileDamage(Damage);
        _projectilesContainer = projectilesContainer;
        SetSpawnPoints(0);
    }

    protected override void UpgradeWeapon()
    {
        SetSpawnPoints(WeaponLevel);
    }

    private void SetProjectileDamage(int damage)
    {
        _bullet.SetDamage(damage);
    }

    private void SetSpawnPoints(int level)
    {
        ShootPoints.Clear();
        foreach (Transform point in _levelShootPoints[level])
        {
            ShootPoints.Add(point);
        }
    }
}