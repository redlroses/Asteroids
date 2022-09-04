using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected readonly List<Transform> ShootPoints = new List<Transform>();
    
    [SerializeField] private int _defaultDamage;
    [SerializeField] private int _maxDamage;
    [SerializeField] private float _defaultCooldown;
    [SerializeField] private float _minCooldown;
    [SerializeField] private int _maxWeaponLevelUpgrade;

    protected int WeaponLevel;

    private GameTimer _gameTimer;
    private int _damage;
    private float _cooldown;

    public int DefaultDamage => _defaultDamage;

    public float DefaultCooldown => _defaultCooldown;

    public float Cooldown
    {
        get => _cooldown;
        private set => _cooldown = value < _minCooldown ? _minCooldown : value;
    }

    public int Damage
    {
        get => _damage;
        private set
        {
            _damage = value > _maxDamage ? _maxDamage : value;
            SetProjectileDamage(_damage);
        }
    }

    public abstract void Shoot();

    public abstract void Initialize();

    protected abstract void Upgrade();

    protected virtual void SetProjectileDamage(int damage)
    {
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void IncreaseDamage(int value)
    {
        Damage += Mathf.Clamp(value, 0, int.MaxValue);
    }

    public void DecreaseCooldown(float time)
    {
        Cooldown -= Mathf.Clamp(time, 0, float.MaxValue);
    }

    public void UpLevel()
    {
        if (++WeaponLevel >= _maxWeaponLevelUpgrade)
        {
            return;
        }

        Upgrade();
    }

    protected void InitializeParameters()
    {
        Cooldown = _defaultCooldown;
        Damage = _defaultDamage;
    }
}