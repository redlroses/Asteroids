using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BonusSpawner), typeof(GameTimer))]
public class Randomizer : MonoBehaviour
{
    private float ShieldChargeSpawnChance
    {
        set => _spawningObjects[1].SpawnChance = value;
    }

    private float WeaponUpgradeSpawnChance
    {
        set => _spawningObjects[0].SpawnChance = value;
    }

    private float BonusSpawnChance
    {
        set
        {
            for (int i = 2; i < 5; i++)
            {
                _spawningObjects[i].SpawnChance = value * GetBonusTypeChance(i - 2);
            }
        }
    }

    public float WeaponBonusSpawnChance
    {
        set
        {
            for (int i = 5; i < 6; i++)
            {
                _spawningObjects[i].SpawnChance = value;
            }
        }
    }

    public Gradient AsteroidChances => _asteroidsGradient;
    public Gradient RarityChances => _rarityGradient;
    public Gradient BonusChance => _bonusGradient;

    [SerializeField] private Gradient _asteroidsGradient;
    [SerializeField] private Gradient _rarityGradient;
    [SerializeField] private Gradient _bonusGradient;
    [SerializeField] private float _bonusSpawnChance;
    [SerializeField] private float _weaponUpgradeSpawnChance;
    [SerializeField] private float _weaponBonusSpawnChance;
    [SerializeField] private float _shieldChargeSpawnChance;
    [SerializeField] private Gradient _spawnChances = new Gradient();
    [SerializeField] private Color32[] _rarityColors;

    private static Randomizer _instance;
    private BonusSpawner _bonusSpawner;
    private List<SpawningObjects> _spawningObjects = new List<SpawningObjects>();

    private const float WeaponBonusSpawnChanceScale = 0.0f;
    private const float WeaponUpgradeSpawnChanceScale = 0.05f;
    private const float ShieldChargeSpawnChanceScale = 0.15f;
    private const float BonusSpawnChanceScale = 0.1f;

    private void Awake()
    {
        _bonusSpawner = GetComponent<BonusSpawner>();
        _instance = this;
    }

    private void Start()
    {
        InitializationChances(_asteroidsGradient);
        InitializationChances(_rarityGradient);
        InitializationChances(_bonusGradient);
        SetSpawnChances();
        SetMainGradientChance();
    }

    private void ResetChances()
    {
        foreach (var item in _spawningObjects)
        {
            item.SetDefaultChance();
        }
    }

    private void SetSpawnChances()
    {
        _spawningObjects.Add(_bonusSpawner.WeaponUpgrade);
        _spawningObjects.Last().DefaultSpawnChance = _weaponUpgradeSpawnChance;

        _spawningObjects.Add(_bonusSpawner.ShieldCharge);
        _spawningObjects.Last().DefaultSpawnChance = _shieldChargeSpawnChance;

        short count = 0;
        foreach (var item in _bonusSpawner.Bonuses)
        {
            _spawningObjects.Add(item.GetComponent<SpawningObjects>());
            _spawningObjects.Last().DefaultSpawnChance = _bonusSpawnChance * GetBonusTypeChance(count);
            count++;
        }

        foreach (var item in _bonusSpawner.WeaponBonus)
        {
            _spawningObjects.Add(item.GetComponent<SpawningObjects>());
            _spawningObjects.Last().DefaultSpawnChance = _weaponBonusSpawnChance;
        }

        int i = 0;
        foreach (var item in _spawningObjects)
        {
            item.IsSpawning = true;
            //Debug.Log($"Индекс: {i}. Название: {item.name}. Шанс: {item.SpawnChance}");
            i++;
        }
    }


    private void SetMainGradientChance()
    {
        List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey>();
        float prevChances = 0;

        for (int i = 0; i < _spawningObjects.Count; i++)
        {
            alphaKeys.Add(new GradientAlphaKey(i / 255f, (_spawningObjects[i].SpawnChance + prevChances) / 100f));
            prevChances += _spawningObjects[i].SpawnChance;
        }

        alphaKeys.Add(new GradientAlphaKey(1f, 1f));
        _spawnChances.alphaKeys = alphaKeys.ToArray();

        // Временный дебаг
        // Debug.Log("---------------------------------------");
        // for (int i = 0; i < _spawnChances.alphaKeys.Length - 1; i++)
        // {
        //     Debug.Log($"Название: {_spawningObjects[Mathf.RoundToInt(_spawnChances.alphaKeys[i].alpha * 255f)]}. Процент: {_spawnChances.alphaKeys[i].time * 100f}");
        // }
    }

    private float GetBonusTypeChance(int count)
    {
        float chance = _bonusGradient.alphaKeys[count + 1].time - _bonusGradient.alphaKeys[count].time;
        return chance;
    }

    // Подготовка системы шансов
    private void InitializationChances(Gradient target)
    {
        List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey> {new GradientAlphaKey(0, 0)};

        for (int i = 0; i < target.colorKeys.Length; i++)
        {
            alphaKeys.Add(new GradientAlphaKey(i / 255f, target.colorKeys[i].time));
        }

        target.alphaKeys = alphaKeys.ToArray();
    }

    public static int GetRandomAsteroidId()
    {
        Color32 color = _instance._asteroidsGradient.Evaluate(Random.value);
        return color.a;
    }

    private Bonus.RarityType GetRandomRarity()
    {
        Color32 color = _rarityGradient.Evaluate(Random.value);
        return (Bonus.RarityType) color.a;
    }

    public static bool TryGetBonus(out GameObject prefab)
    {
        prefab = null;
        float randomValue = Random.value;
        Color32 color = _instance._spawnChances.Evaluate(randomValue);
        int index = color.a;

        if (index == 255)
        {
            return false;
        }

        prefab = _instance._spawningObjects[index].gameObject;

        //Debug.Log($"Индекс: {index}. Название: {prefab.name}. Шанс: {_instance._spawningObjects[index].SpawnChance}");
        if (prefab.TryGetComponent(out Bonus bonus))
        {
            bonus.Rarity = _instance.GetRandomRarity();
            bonus.GetComponent<SpriteRenderer>().color = _instance._rarityColors[(int) bonus.Rarity];
        }

        return true;
    }

    private void WeaponUpgradeOnWeaponUpgradeSpawns(object sender, bool flag)
    {
        _bonusSpawner.WeaponUpgrade.IsSpawning = flag;
        SetMainGradientChance();
    }

    private void SetWeaponUpgradeIsSpawning(object sender, bool flag)
    {
        GameTimer.OnNextWeaponUpgrade -= WeaponUpgradeOnWeaponUpgradeSpawns;
        WeaponUpgradeOnWeaponUpgradeSpawns(null, flag);
    }

    private void MoverOnMaxPlayerSpeed(object sender, EventArgs e)
    {
        _bonusSpawner.SpeedBonus.IsSpawning = false;
        SetMainGradientChance();
    }

    private void NewWeaponOnMaxDamage(object sender, EventArgs e)
    {
        _bonusSpawner.DamageBonus.IsSpawning = false;
    }

    private void NewWeaponOnMinCooldown(object sender, EventArgs e)
    {
        _bonusSpawner.CooldownBonus.IsSpawning = false;
    }
    
    private void OnEnable()
    {
        Shield.OnFullCharges += ShieldSystemOnOnFullCharges;
        WeaponUpgrade.OnWeaponUpgradeSpawns += WeaponUpgradeOnWeaponUpgradeSpawns;
        GameTimer.OnUpSpawnChances += GameTimerOnUpSpawnChances;
        GameTimer.OnNextWeaponUpgrade += WeaponUpgradeOnWeaponUpgradeSpawns;
        GameTimer.OnUpDifficulty += GameTimerOnUpSpawnChances;
        Weapon.OnMaxWeaponLevel += SetWeaponUpgradeIsSpawning;
        Weapon.OnMaxDamage += NewWeaponOnMaxDamage;
        Weapon.OnMinCooldown += NewWeaponOnMinCooldown;
        Mover.OnMaxPlayerSpeed += MoverOnMaxPlayerSpeed;
    }

    private void OnDisable()
    {
        Shield.OnFullCharges -= ShieldSystemOnOnFullCharges;
        WeaponUpgrade.OnWeaponUpgradeSpawns -= WeaponUpgradeOnWeaponUpgradeSpawns;
        GameTimer.OnUpSpawnChances -= GameTimerOnUpSpawnChances;
        GameTimer.OnNextWeaponUpgrade -= WeaponUpgradeOnWeaponUpgradeSpawns;
        GameTimer.OnUpDifficulty -= GameTimerOnUpSpawnChances;
        Weapon.OnMaxWeaponLevel -= SetWeaponUpgradeIsSpawning;
        Weapon.OnMaxDamage -= NewWeaponOnMaxDamage;
        Mover.OnMaxPlayerSpeed -= MoverOnMaxPlayerSpeed;
    }

    private void GameTimerOnUpSpawnChances(object sender, int level)
    {
        WeaponBonusSpawnChance = _weaponBonusSpawnChance + level * WeaponBonusSpawnChanceScale;
        WeaponUpgradeSpawnChance = _weaponUpgradeSpawnChance + level * WeaponUpgradeSpawnChanceScale;
        ShieldChargeSpawnChance = _shieldChargeSpawnChance + level * ShieldChargeSpawnChanceScale;
        BonusSpawnChance = _bonusSpawnChance + level * BonusSpawnChanceScale;
        SetMainGradientChance();
    }

    private void ShieldSystemOnOnFullCharges(object sender, bool flag)
    {
        _bonusSpawner.ShieldCharge.IsSpawning = flag;
        SetMainGradientChance();
    }
}