using System;
using UnityEngine;

[RequireComponent(typeof(LaserBulletPool))]
public class Cannon : Weapon
{
    [SerializeField] private LaserBullet _bullet;
    [SerializeField] private Transform[] _levelShootPoints;
    
    private LaserBulletPool _pool;

    private void Awake()
    {
        _pool = GetComponent<LaserBulletPool>();
    }

    public override void Shoot()
    {
        foreach (var point in ShootPoints)
        {
            _pool.EnableCopy(point.position, point.rotation);
        }
    }

    public override void Initialize()
    {
        InitializeParameters();
        SetProjectileDamage(Damage);
        SetSpawnPoints(0);
    }

    protected override void Upgrade()
    {
        SetSpawnPoints(WeaponLevel);
    }

    protected override void SetProjectileDamage(int damage)
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