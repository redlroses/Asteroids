using System;
using UnityEngine;

public abstract class Bonus : MonoBehaviour, IPoolable<Bonus>
{
    public enum Types
    {
        Damage, Cooldown, Speed, WeaponUpgrade, ShieldCharge
    }

    public Types Type => _type;
    
    public event Action<Bonus> OnDisabled;
    
    [SerializeField, Min(0f)] protected float _bonusScaleFactor;
    [SerializeField] protected Types _type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryApplyBonus(collision);
        OnDisabled?.Invoke(this);
    }

    public bool GetActiveSelf()
    {
        return gameObject.activeSelf;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    
    protected abstract void TryApplyBonus(Collider2D collision);
}