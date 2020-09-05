using System;
using UnityEngine;

public abstract class SpawningObjects : MonoBehaviour
{
    public bool IsSpawning
    {
        get => _isSpawning;
        set
        {
            //Debug.Log($"Спавнить? {value}");
            _isSpawning = value;
        }
    }

    public float DefaultSpawnChance
    {
        set
        {
            _defaultSpawnChance = value;
            _spawnChance = value;
        }
    }

    public float SpawnChance
    {
        get => _spawnChance * Convert.ToInt32(_isSpawning);
        set { _spawnChance = value; }
    }

    protected float _spawnChance;
    protected float _defaultSpawnChance;
    protected bool _isSpawning = true;

    public void SetDefaultChance()
    {
        _spawnChance = _defaultSpawnChance;
    }

    protected abstract void ApplyBonus(Player player);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            ApplyBonus(player);
            Destroy(gameObject);
        }
    }
}