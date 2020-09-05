using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Shield : Player
{
    public int ShieldCharges
    {
        get => _shieldCharges;
        set
        {
            if (value < 0) return;
            
            _shieldCharges = value;
            
            if (_shieldCharges >= _maxShieldCharges)
            {
                _shieldCharges = _maxShieldCharges;
                OnFullCharges?.Invoke(this, false);
                //Debug.Log("Полный заряд");
            }
            
            OnChargesChanges?.Invoke(this, ShieldCharges);
        }
    }

    public float ShieldRechargeTime
    {
        get => _shieldCharges;
        set
        {
            _shieldRechargeTime = value;
            _shieldRechargeTimeWaitForSeconds = new WaitForSeconds(value);
        }
    }
    
    public static event EventHandler<bool> OnFullCharges;
    public static event EventHandler<int> OnChargesChanges;
    public static event EventHandler OnLoseGame;

    public UnityEvent OnLoseGameUnityEvent;
    
    [SerializeField] private int _shieldCharges = 1;
    [SerializeField] private float _shieldRechargeTime = 2.75f;
    [SerializeField] private int _maxShieldCharges = 3;
    
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _shieldRechargeTimeWaitForSeconds;
    private bool _active = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ShieldRechargeTime = _shieldRechargeTime;
        OnChargesChanges?.Invoke(this, ShieldCharges);
    }

    private void Hit()
    {
        if (_active && ShieldCharges > 0)
        {
            ShieldCharges--;
            OnFullCharges?.Invoke(this, true);
            //Debug.Log("Неполный заряд");
            StartCoroutine(ShieldRecharge());
        }
        else
        {
            LoseGame();
        }
    }

    private void LoseGame()
    {
        OnLoseGame?.Invoke(this,EventArgs.Empty);
        OnLoseGameUnityEvent?.Invoke();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Проверка на касание с игроком
        if (!collision.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            return;
        }
        
        asteroid.CollapseByShield();
        Hit();
    }

    private IEnumerator ShieldRecharge()
    {
        _active = false;
        yield return _shieldRechargeTimeWaitForSeconds;
        _active = true;
    }
}
