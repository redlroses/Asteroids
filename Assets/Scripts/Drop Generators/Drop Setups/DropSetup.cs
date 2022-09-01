using UnityEngine;

[CreateAssetMenu(fileName = "NewDropSetup", menuName = "ScriptableObjects/DropSetup", order = 51)]
public class DropSetup : ScriptableObject
{
    [SerializeField] private GameObject _dropTarget;
    [SerializeField] private float _currentChance;

    public float CurrentChance { 
        get => _currentChance;
        protected set => _currentChance = value; 
    }

    public GameObject DropTarget => _dropTarget;

    public virtual void Initialize()
    {
        
    }
}
