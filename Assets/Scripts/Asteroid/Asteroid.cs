using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AsteroidMover))]
public class Asteroid : MonoBehaviour, IPoolable<Asteroid>
{
    public enum SizeType { Small, Medium, Big }

    public event Action<Asteroid> Disabled;

    [SerializeField] private int _hitPoints;
    [SerializeField] private int _scoreAmount;
    [SerializeField] private SizeType _size;
    [SerializeField] private Transform[] _spawnPositions;

    private int _currentHitPoints;
    private bool _isDestroyedByPlayer = false;
    private ScoreCounter _scoreCounter;
    private AsteroidMover _mover;

    public AsteroidMover Mover => _mover;
    public SizeType Size => _size;
    public Transform[] SpawnPositions => _spawnPositions;
    public bool IsDestroyedByPlayer => _isDestroyedByPlayer;
    public int ScoreAmount => _scoreAmount;

    private void Awake()
    {
        _mover = GetComponent<AsteroidMover>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeSelf && other.TryGetComponent(out OutOfBoundsDestroyer destroyer))
        {
            Disabled?.Invoke(this);
        }
    }

    public void Enable()
    {
        _isDestroyedByPlayer = false;
        _currentHitPoints = _hitPoints;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            throw new ArgumentException("Damage of projectile low then zero", nameof(damage));
        }

        _currentHitPoints -= damage;

        if (_currentHitPoints > 0 || _isDestroyedByPlayer)
            return;

        _isDestroyedByPlayer = true;
        Disabled?.Invoke(this);
    }

    public void CollapseByShield()
    {
        Disabled?.Invoke(this);
    }
}
