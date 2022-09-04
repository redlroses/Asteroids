using System;
using UnityEngine;

public abstract class Bonus : MonoBehaviour, IPoolable<Bonus>
{
    public enum Types
    {
        Damage, Cooldown, Speed, WeaponUpgrade, ShieldCharge
    }

    public Types Type => _type;
    
    public event Action<Bonus> Disabled;
    
    [SerializeField, Min(0f)] protected float _bonusScaleFactor;
    [SerializeField] protected Types _type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryApply(collision);
        Disabled?.Invoke(this);
    }

    public bool GetActiveSelf()
    {
        return gameObject.activeSelf;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    
    protected abstract void TryApply(Collider2D collision);
}