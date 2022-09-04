using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private const float MillisecondToSecond = 1000f;

    [SerializeField] private Weapon _weapon;
    [SerializeField] private Coroutine _shooting;
    [SerializeField] bool _isShooting = false;

    private WaitForSeconds _waitForWeaponCooldown;

    public Weapon Weapon => _weapon;

    public void SetWeapon(Weapon newWeapon)
    {
        _weapon = newWeapon;
        SetCooldown(_weapon.Cooldown);
        StartShooting();
    }

    private void StartShooting()
    {
        _isShooting = true;

        if (_shooting == null)
        {
            _shooting = StartCoroutine(Shooting());
        }
    }

    private IEnumerator Shooting()
    {
        while (_isShooting)
        {
            yield return _waitForWeaponCooldown;
            _weapon.Shoot();
        }
    }

    private void SetCooldown(float cooldown)
    {
        _waitForWeaponCooldown = new WaitForSeconds(cooldown / MillisecondToSecond);
    }
}
