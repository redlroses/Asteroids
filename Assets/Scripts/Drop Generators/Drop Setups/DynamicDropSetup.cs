using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDropSetup", menuName = "ScriptableObjects/DynamicDropSetup", order = 51)]
public class DynamicDropSetup : DropSetup
{
    [SerializeField] private float _minChance;
    [SerializeField] private float _maxChance;
    [SerializeField] private float _chanceStep;

    public float MaxChance => _maxChance;
    protected bool IsMaxChance { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        CheckIsCorrectChances();
        IsMaxChance = false;
        Reset();
    }

    public void Reset()
    {
        CurrentChance = _minChance;
    }
    
    protected void UpChance()
    {
        if (IsMaxChance)
        {
            return;
        }
        
        CurrentChance += _chanceStep;

        if (CurrentChance < _maxChance)
        {
            return;
        }
        
        IsMaxChance = true;
        CurrentChance = _maxChance;
    }

    private void CheckIsCorrectChances()
    {
        if (_minChance > _maxChance)
        {
            throw new ArgumentException("The minimum chance cannot be greater than the maximum", nameof(_minChance));
        }
    }
}
