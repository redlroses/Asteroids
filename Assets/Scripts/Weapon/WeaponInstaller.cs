using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInstaller : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Shooter _shooter;
    [SerializeField] private DataContainer _dataContainer;
    [SerializeField] private Transform _projectiles;

    private void Awake()
    {
        if (_shooter.Weapon == null)
        {
            SetDefaultWeapon();
        }
    }

    private bool TryInstallWeapon(Type weaponType) 
    {
        var currentWeapon = _weapons.First(weapon => weapon.GetType() == weaponType);

        if (currentWeapon == null)
        {
            return false;
        }

        if (_shooter.Weapon != null)
        {
            _shooter.Weapon.Remove();
        }
        
        currentWeapon = Instantiate(currentWeapon, _shooter.transform);
        currentWeapon.Initialize(_projectiles);
        _shooter.SetWeapon(currentWeapon);
        _shooter.StartShooting();
        
        return true;
    }
    
    [ContextMenu("SetCannon")]
    private void SetDefaultCannon()
    {
        TryInstallWeapon(typeof(Cannon));
    }
    
    [ContextMenu("SetLaser")]
    private void SetDefaultLaser()
    {
        TryInstallWeapon(typeof(Laser));
    }

    private void SetDefaultWeapon()
    {
        var chosenWeaponType = _dataContainer.GetChosenWeapon();
        TryInstallWeapon(chosenWeaponType);
    }
}
