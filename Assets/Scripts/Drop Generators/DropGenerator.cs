using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropGenerator<T> : MonoBehaviour where T : Component
{
    private const float MaxChance = 100f;
    private const float MinChance = 0f;
    
    [SerializeField] protected List<DropSetup> _dropSetups;

    private List<T> _dropTargets;
    private int _setupsCount;

    private void Awake()
    {
        Initialize();
        InitializeAwake();
    }

    private void Start()
    {
        CheckIsCorrectChances();
        InitializeStart();
    }

    public bool TryGetDrop(out T randomObject)
    {
        float randomValue = Random.Range(MinChance, MaxChance);
        int currentSetup = _setupsCount;
        float sumChances = GetCurrentSumChances();

        while (sumChances > randomValue)
        {
            sumChances -= _dropSetups[currentSetup - 1].CurrentChance;
            currentSetup--;
        }
    
        randomObject = currentSetup == _setupsCount ? null : _dropTargets[currentSetup];
        return currentSetup != _setupsCount;
    }

    protected virtual void InitializeAwake()
    {
    }
    
    protected virtual void InitializeStart()
    {
    }

    protected void ResetChances<TD>(Func<TD, bool> filter) where TD : DynamicDropSetup
    {
        if (TryGetDynamicSetups<TD>(out var dynamicSetups) == false)
        {
            return;
        }
        
        var filteredSetup = dynamicSetups.Where(filter ?? (drop => true));
        
        foreach (var setup in filteredSetup)
        {
            setup.Reset();
        }
    }

    private bool TryGetDynamicSetups<TD>(out IEnumerable<TD> dynamicDropSetups) where TD : DynamicDropSetup
    {
        bool isDynamic = _dropSetups.First() is DynamicDropSetup;
        dynamicDropSetups = isDynamic ? _dropSetups.Cast<TD>() : null;
        return isDynamic;
    }

    private void Initialize()
    {
        _setupsCount = _dropSetups.Count;
        _dropTargets = new List<T>(_dropSetups.Select(drop => drop.DropTarget.GetComponent<T>()));
    }

    private float GetCurrentSumChances()
    {
        return _dropSetups.Sum(drop => drop.CurrentChance);
    }
    
    private void CheckIsCorrectChances()
    {
        float sumMinChances = _dropSetups.Sum(chance => chance.CurrentChance);

        if (sumMinChances > MaxChance)
        {
            throw new ArgumentOutOfRangeException(nameof(sumMinChances));
        }

        if (TryGetDynamicSetups<DynamicDropSetup>(out var dynamicSetups) == false)
        {
            return;
        }
        
        float sumMaxChances = dynamicSetups.Sum(chance => chance.MaxChance);

        if (sumMaxChances > MaxChance)
        {
            throw new ArgumentOutOfRangeException(nameof(sumMaxChances));
        }
    }
}
