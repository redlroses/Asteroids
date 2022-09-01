using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IPoolable<Asteroid>
{
    public enum SizeType { Small, Medium, Big }

    public event Action<Asteroid> OnDisabled;

    [SerializeField] private int _hitPoints;
    [SerializeField] private int _scoreAmount;
    [SerializeField] private Vector2 _moveSpeedRange;
    [SerializeField] private Vector2 _rotationSpeedRange;
    [SerializeField] private SizeType _size;
    [SerializeField] private Transform[] _spawnPositions;

    private int _currentHitPoints;
    private Rigidbody2D _rigidbody;
    private ScoreCounter _scoreCounter;
    private bool _isDestroyedByPlayer = false;

    public SizeType Size => _size;
    public Transform[] SpawnPositions => _spawnPositions;
    public Vector2 Velocity => _rigidbody.velocity;
    public bool IsDestroyedByPlayer => _isDestroyedByPlayer;
    public int ScoreAmount => _scoreAmount;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeSelf && other.TryGetComponent(out OutOfBoundsDestroyer destroyer))
        {
            Disable();
        }
    }

    public bool GetActiveSelf()
    {
        return gameObject.activeSelf;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Enable()
    {
        _isDestroyedByPlayer = false;
        _currentHitPoints = _hitPoints;
        Move();
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
        Disable();
    }

    public void CollapseByShield()
    {
        Disable();
    }

    private void Move()
    {
        _rigidbody.AddRelativeForce(Vector2.up * Random.Range(_moveSpeedRange.x, _moveSpeedRange.y),
            ForceMode2D.Impulse);
        _rigidbody.AddTorque(Random.Range(_rotationSpeedRange.x, _rotationSpeedRange.y), ForceMode2D.Impulse);
    }

    private void Disable()
    {
        OnDisabled?.Invoke(this);
    }
}
