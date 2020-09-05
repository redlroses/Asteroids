using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public int BonusesCount => _bonusPrefabs.Count;
    public List<GameObject> Bonuses => _bonusPrefabs;
    public DamageBonus DamageBonus => _bonusPrefabs[0].GetComponent<DamageBonus>();
    public CooldownBonus CooldownBonus => _bonusPrefabs[1].GetComponent<CooldownBonus>();
    public SpeedBonus SpeedBonus => _bonusPrefabs[2].GetComponent<SpeedBonus>();
    public WeaponUpgrade WeaponUpgrade => _weaponUpgradePrefab.GetComponent<WeaponUpgrade>();
    public List<GameObject> WeaponBonus => _weaponBonus;
    public ShieldCharge ShieldCharge => _shieldChargePrefab.GetComponent<ShieldCharge>();
    
    [SerializeField] private GameObject _weaponUpgradePrefab;
    [SerializeField] private GameObject _shieldChargePrefab;
    [SerializeField] private List<GameObject> _bonusPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _weaponBonus = new List<GameObject>();
    [SerializeField] private Transform _bonusParent;

    private void AsteroidOnAsteroidDestroy(object sender, int size)
    {
        var asteroid = (Asteroid)sender;

        if (Randomizer.TryGetBonus(out GameObject spawningObject))
        {
            Instantiate(spawningObject, asteroid.transform.position, Quaternion.identity, _bonusParent);
        }
    }

    private void OnEnable()
    { 
        Asteroid.OnAsteroidDestroy += AsteroidOnAsteroidDestroy;
    }

    private void OnDisable()
    {
        Asteroid.OnAsteroidDestroy -= AsteroidOnAsteroidDestroy;
    }
}
