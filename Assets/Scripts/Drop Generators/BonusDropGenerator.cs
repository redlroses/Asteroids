using System.Linq;
using UnityEngine;

public class BonusDropGenerator : DropGenerator<Bonus>
{
    [SerializeField] private GameTimer _gameTimer;

    protected override void InitializeAwake()
    {
        foreach (var dropSetup in _dropSetups.Cast<BonusDropSetup>())
        {
            dropSetup.Initialize(_gameTimer);
        }
    }

    public void ResetWeaponUpgradeChances()
    {
        ResetChances<BonusDropSetup>(drop => drop.Type == Bonus.Types.WeaponUpgrade);
    }
    
    [ContextMenu("Test")]
    public void Test()
    {
        for (int i = 0; i < 100000; i++)
        {
            if (TryGetDrop(out Bonus bonus))
            {
                Debug.Log(bonus.name);
            }
            else
            {
                Debug.Log("null");
            }
        }
    }
}
