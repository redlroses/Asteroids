using UnityEngine;

public class Cannon : Weapon
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _projectilesParent;
    [SerializeField] private Transform[] _levelSpawnPoints = default;
    
    private LaserBullet _bullet;

    protected override void Initialize()
    {
        SetSpawnPoints(0);
    }

    protected override void Shoot()
    {
        foreach (var point in spawnPoints)
        {
            Instantiate(_bulletPrefab, point.position, point.rotation, _projectilesParent);
        }
    }

    protected override void SetProjectileDamage()
    {
        _bullet.SetDamage(Damage);
    }

    protected override void UpgradeWeapon()
    {
        weaponLevel++;

        if (weaponLevel == _levelSpawnPoints.Length - 1)
        {
            MaxLevel();
        }
        
        SetSpawnPoints(weaponLevel);
    }

    private void Awake()
    {
        _bullet = _bulletPrefab.GetComponent<LaserBullet>();
        _projectilesParent = GameObject.FindWithTag("Projectile").transform;
    }
    
    private void SetSpawnPoints(int level)
    {
        spawnPoints.Clear();
        foreach (Transform point in _levelSpawnPoints[level])
        {
            spawnPoints.Add(point);
        }
    }
}