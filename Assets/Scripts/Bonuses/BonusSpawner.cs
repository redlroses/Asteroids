using UnityEngine;

[RequireComponent(typeof(BonusDropGenerator), typeof(BonusPool))]
public class BonusSpawner : MonoBehaviour
{
    [SerializeField] private AsteroidPool _asteroidPool;

    private BonusDropGenerator _dropGenerator;
    private BonusPool _pool;

    private void Awake()
    {
        _pool = GetComponent<BonusPool>();
        _dropGenerator = GetComponent<BonusDropGenerator>();
    }

    private void OnEnable()
    { 
        _asteroidPool.AsteroidDestroyed += SpawnBonus;
    }

    private void OnDisable()
    {
        _asteroidPool.AsteroidDestroyed -= SpawnBonus;
    }

    private void SpawnBonus(Asteroid asteroid)
    {
        if (_dropGenerator.TryGetDrop(out Bonus bonus) == false)
        {
            return;
        }
        
        _pool.EnableCopy(asteroid.transform.position, Quaternion.identity, copy => copy.Type == bonus.Type);

        if (bonus.Type == Bonus.Types.WeaponUpgrade)
        {
            _dropGenerator.ResetWeaponUpgradeChances();
        }
    }
}
