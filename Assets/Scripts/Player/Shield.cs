using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Shield : MonoBehaviour
{
    public event Action ShipDead;

    [SerializeField] private int _charges = 1;
    [SerializeField] private float _rechargeSeconds = 2.75f;
    [SerializeField] private int _maxCharges = 3;
    [SerializeField] private UnityEvent<int> _onChargesChanges;

    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _rechargeTime;
    private bool _active = true;

    public int Charges
    {
        get => _charges;
        private set
        {
            if (value < 0) return;
            
            _charges = value;
            
            if (_charges >= _maxCharges)
            {
                _charges = _maxCharges;
            }
            
            _onChargesChanges?.Invoke(Charges);
        }
    }

    public float RechargeTimeWait
    {
        get => _charges;
        private set
        {
            _rechargeSeconds = value;
            _rechargeTime = new WaitForSeconds(value);
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        RechargeTimeWait = _rechargeSeconds;
        _onChargesChanges?.Invoke(Charges);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Asteroid asteroid) == false)
        {
            return;
        }
        
        asteroid.CollapseByShield();
        Hit();
    }

    public void AddCharge(int amount)
    {
        Charges += Mathf.Clamp(amount, 0, _maxCharges);
    }
    
    private void Hit()
    {
        if (_active && Charges > 0)
        {
            Charges--;
            StartCoroutine(ShieldRecharge());
        }
        else
        {
            Dead();
        }
    }

    private void Dead()
    {
        ShipDead?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator ShieldRecharge()
    {
        _active = false;
        yield return _rechargeTime;
        _active = true;
    }
}
