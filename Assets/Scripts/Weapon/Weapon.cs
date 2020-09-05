using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Player
{
    public enum WeaponClass
    {
        None = 0,
        Cannon = 1,
        Laser = 2
    }

    public WeaponClass CurrentWeaponClass => currentWeaponClass;
    
    public int DefaultDamage => _defaultDamage;

    public float DefaultCooldown => _defaultCooldown;

    public float Cooldown
    {
        get => _cooldown;
        set
        {
            _cooldown = value;

            if (_cooldown <= _minCooldown)
            {
                _cooldown = _minCooldown;
                OnMinCooldown?.Invoke(this, EventArgs.Empty);
            }

            cooldownWaitForSeconds = new WaitForSeconds(_cooldown / 1000f);
        }
    }

    public int Damage
    {
        get => _damage;
        set
        {
            _damage = value;

            if (_damage >= _maxDamage)
            {
                _damage = _maxDamage;
                OnMaxDamage?.Invoke(this, EventArgs.Empty);
            }

            SetProjectileDamage();
        }
    }

    public static event EventHandler OnMaxDamage;
    public static event EventHandler OnMinCooldown;
    public static event EventHandler<bool> OnMaxWeaponLevel;

    [SerializeField] protected WeaponClass currentWeaponClass;

    [SerializeField] private int _defaultDamage = default;
    [SerializeField] private int _maxDamage = default;
    [SerializeField] private float _defaultCooldown = default;
    [SerializeField] private float _minCooldown = default;
    [SerializeField] private Coroutine shooting = default;
    [SerializeField] private WaitForSeconds cooldownWaitForSeconds = default;

    protected List<Transform> spawnPoints = new List<Transform>();
    protected int weaponLevel;

    private int _damage;
    private float _cooldown;

    public void LevelUp()
    {
        UpgradeWeapon();
    }

    private void StartShooting()
    {
        shooting = StartCoroutine(Shooting());
    }

    protected abstract void Shoot();

    protected virtual void SetProjectileDamage()
    {
    }

    protected virtual void Initialize()
    {
    }

    protected abstract void UpgradeWeapon();
    
    protected void MaxLevel()
    {
        OnMaxWeaponLevel?.Invoke(this, false);
    }

    private void Start()
    {
        InitializeParameters();
        StartShooting();
        Initialize();
    }

    private void InitializeParameters()
    {
        Cooldown = _defaultCooldown;
        Damage = _defaultDamage;
    }
    
    private IEnumerator Shooting()
    {
        while (true)
        {
            Shoot();
            Debug.Log($"Shoot: {Damage} damage, {Cooldown} cool down");
            yield return cooldownWaitForSeconds;
        }
    }
}