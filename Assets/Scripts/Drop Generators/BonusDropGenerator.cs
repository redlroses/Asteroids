using System.Linq;
using UnityEngine;

public class BonusDropGenerator : DropGenerator<Bonus>
{
    [SerializeField] private GameTimer _gameTimer;

    public void ResetWeaponUpgradeChances()
    {
        ResetChances<BonusDropSetup>(drop => drop.Type == Bonus.Types.WeaponUpgrade);
    }

    protected override void InitializeAwake()
    {
        foreach (var dropSetup in _dropSetups.Cast<BonusDropSetup>())
        {
            dropSetup.Initialize(_gameTimer);
        }
    }
}
