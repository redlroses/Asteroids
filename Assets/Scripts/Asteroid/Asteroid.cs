using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    public enum Size { Small, Medium, Big };

    public static event EventHandler<int> OnAsteroidDestroy;

    [SerializeField] private int _hitPoints = 0;
    [SerializeField] private float _moveSpeed = 0;
    [SerializeField] private float _rotationsSpeed = 0;
    [SerializeField] private Size _currentSize;
    [SerializeField] private GameObject _asteroidPrefub = null;
    [SerializeField] private float _fragmentationRange;
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private Transform _asteroidsParent;
    [SerializeField] private GameObject _asteroidScorePrefab;

    private Rigidbody2D _rigidbody;
    private bool _isDestroyed = false; //Используется для исправления бага со спавном нескольких пар астеройдов

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _asteroidsParent = transform.parent;
        Move();
        Destroy(gameObject, 25f);
    }

    private void Move()
    {
        _rigidbody.AddRelativeForce(Vector2.up * _moveSpeed * Random.Range(1f, 1.5f), ForceMode2D.Impulse);
        _rigidbody.AddTorque(_rotationsSpeed * Random.Range(-1f, 1f), ForceMode2D.Impulse);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) Debug.LogError("Damage of projectile low then zero");

        _hitPoints -= damage;

        if (_hitPoints <= 0 && !_isDestroyed)
        {
            _isDestroyed = true;
            Collapse(); 
        }
    }

    //Разрушение астеройда от оружия
    private void Collapse()
    {
        if (_currentSize > 0)
        {
            SpawnNewAsteroids();
        }

        OnAsteroidDestroy?.Invoke(this, (int)_currentSize);
        
        // Создание информационного угасающего текста с количеством очков за уничтожение астероида 
        var asteroidScore = Instantiate(_asteroidScorePrefab, transform.position, Quaternion.identity).GetComponent<AsteroidScore>();
        asteroidScore.SetScoreText(ScoreCounter.ScoreBySize((int)_currentSize));
        
        Destroy(gameObject);
    }
    
    //Разрушение астеройда от соприкосновения со щитом
    public void CollapseByShield()
    {
        _isDestroyed = true;
        Destroy(gameObject);
    }

    private void SpawnNewAsteroids()
    {
        bool leftOrRight = true;
        for (int i = 0; i < 2; i++)
        {
            Quaternion asteroidRotation = RandomizeDirection(_fragmentationRange, leftOrRight);
            Instantiate(_asteroidPrefub, _spawnPositions[i].position, asteroidRotation, _asteroidsParent);
            leftOrRight = !leftOrRight;
        }
    }

    //Выбор случайного вектора разлёта созданных астеройдов
    private Quaternion RandomizeDirection(float limit, bool leftOrRight)
    {
        Vector2 prevDirection = _rigidbody.velocity;
        Vector2 perpendicular = Vector2.Perpendicular(prevDirection).normalized;
        Vector2 newDirection = prevDirection + perpendicular * Random.Range(0, limit) * Convert.ToInt32(leftOrRight);
        float angle = Vector2.SignedAngle(Vector2.up, newDirection);
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out OutOfBoundsDestroyer destroyer))
        {
            Destroy(gameObject);
        }
    }
}
