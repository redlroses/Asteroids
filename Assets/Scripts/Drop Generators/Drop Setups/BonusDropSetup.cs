using UnityEngine;

[CreateAssetMenu(fileName = "NewDropSetup", menuName = "ScriptableObjects/BonusDropSetup", order = 51)]
public class BonusDropSetup : DynamicDropSetup
{
    private GameTimer _gameTimer;
    private Bonus.Types _type;

    public Bonus.Types Type => _type;
    
    public void Initialize(GameTimer gameTimer)
    {
        base.Initialize();
        _gameTimer = gameTimer;
        _gameTimer.OnUpSpawnChances += TryUpSpawnChance;
        _type = DropTarget.GetComponent<Bonus>().Type;
    }

    private void TryUpSpawnChance(int time)
    {
        if (IsMaxChance)
        {
            _gameTimer.OnUpSpawnChances -= TryUpSpawnChance;
        }
        
        UpChance();
    }
}
