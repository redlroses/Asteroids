using System;
using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private OldWeapon _defaultOldWeapon;
    [SerializeField] private Laser _laser;
    [SerializeField] private Cannon _cannon;
    
    private WaitForSeconds _timeBetweenShoots;
    private bool _isShooting;
    private IShootable _currentWeapon;

    public OldWeapon GetOldWeapon => _defaultOldWeapon;
    
    private void Start()
    {
        _currentWeapon = (IShootable)_defaultOldWeapon;
        SetTimeBetweenShots(_defaultOldWeapon.GetShotsCoolDown);
        StartCoroutine(Shooting());
    }

    public void SetCurrentWeapon(OldWeapon oldWeapon, float delay)
    {
        _defaultOldWeapon = (OldWeapon)_currentWeapon;
        _currentWeapon = (IShootable)oldWeapon;
        SetTimeBetweenShots(oldWeapon.GetShotsCoolDown);
        StartCoroutine(SwitchToDefault(delay));
        Debug.Log("Weapon Set");
    }

    private void WeaponShoot()
    {
        _currentWeapon.Shoot();
    }

    private void StartShooting(object sender, EventArgs a)
    {
        _isShooting = true;
    }

    private void FinishShooting(object sender, EventArgs a)
    {
        _isShooting = false;
    }

    private void OnEnable()
    {
        PlayerShooter.OnStartShoot += StartShooting;
        PlayerShooter.OnFinishShoot += FinishShooting;
    }

    private void OnDisable()
    {
        PlayerShooter.OnStartShoot -= StartShooting;
        PlayerShooter.OnFinishShoot -= FinishShooting;
    }

    public void SetTimeBetweenShots(float delay)
    {
        _timeBetweenShoots = new WaitForSeconds(delay / 1000f);
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            if (_isShooting)
            {
                WeaponShoot();
                yield return _timeBetweenShoots;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator SwitchToDefault(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Default Set");

        _currentWeapon = (IShootable)_defaultOldWeapon;
        SetTimeBetweenShots(_defaultOldWeapon.GetShotsCoolDown);
    }
}
