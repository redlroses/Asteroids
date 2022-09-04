using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInstaller : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Shooter _shooter;
    [SerializeField] private DataContainer _dataContainer;

    private void Awake()
    {
        if (_shooter.Weapon == null)
        {
            InstallSaved();
        }
        else
        {
            InstallCurrent();
        }
    }

    public bool TryInstall<T>() where T : Weapon
    {
        var currentWeapon = _weapons.First(weapon => weapon.GetType() == typeof(T));

        if (currentWeapon == null)
        {
            return false;
        }

        if (_shooter.Weapon != null)
        {
            _shooter.Weapon.Remove();
        }
        
        Install(currentWeapon);
        return true;
    }

    [ContextMenu("SetCannon")]
    private void SetCannon()
    {
        TryInstall<Cannon>();
    }
    
    [ContextMenu("SetLaser")]
    private void SettLaser()
    {
        TryInstall<Laser>();
    }

    private void InstallSaved()
    {
        var chosenWeaponType = _dataContainer.GetChosenWeapon();
        Install(_weapons.First(weapon => weapon.GetType() == chosenWeaponType));
    }

    private void InstallCurrent()
    {
        Install(_shooter.Weapon);
    }

    private void Install(Weapon weapon)
    {
        weapon = Instantiate(weapon, _shooter.transform);
        weapon.Initialize();
        _shooter.SetWeapon(weapon);
    }
}

