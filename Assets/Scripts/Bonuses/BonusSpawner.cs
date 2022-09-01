using UnityEngine;

[RequireComponent(typeof(BonusDropGenerator))]
public class BonusSpawner : ObjectPool<Bonus>
{
    [SerializeField] private AsteroidsPool _asteroidsPool;

    private BonusDropGenerator _dropGenerator;

    private void OnEnable()
    { 
        _asteroidsPool.OnAsteroidDestroyed += SpawnBonus;
    }

    private void OnDisable()
    {
        _asteroidsPool.OnAsteroidDestroyed -= SpawnBonus;
    }

    protected override void InitializeAwake()
    {
        _dropGenerator = GetComponent<BonusDropGenerator>();
    }

    private void SpawnBonus(Vector3 position, int score)
    {
        if (_dropGenerator.TryGetDrop(out Bonus bonus) == false)
        {
            return;
        }
        
        EnableCopy(position, Quaternion.identity, copy => copy.Type == bonus.Type);

        if (bonus.Type == Bonus.Types.WeaponUpgrade)
        {
            _dropGenerator.ResetWeaponUpgradeChances();
        }
    }
}
