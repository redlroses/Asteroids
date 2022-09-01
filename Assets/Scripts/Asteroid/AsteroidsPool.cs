using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AsteroidDropGenerator))]
public class AsteroidsPool : ObjectPool<Asteroid>
{
    public event Action<Vector3, int> OnAsteroidDestroyed;
    
    private readonly float _millisecondsToSeconds = 1000f;
    private readonly List<Transform> _lookAtTargets = new List<Transform>();
    private readonly List<Transform> _spawnTargets = new List<Transform>();

    [SerializeField] private bool _isSpawning = true;
    [SerializeField] private int _maxSpawnTime;
    [SerializeField] private int _minSpawnTime;
    [SerializeField] private int _difficultyLevelsAmount;
    [SerializeField] private Transform _lookAtTargetsParent;
    [SerializeField] private Transform _spawnTargetsParent;
    [SerializeField] private GameTimer _gameTimer;

    private float _spawnCoolDownFloat;
    private AsteroidDropGenerator _dropGenerator;
    private Coroutine _spawnerCoroutine;
    private WaitForSeconds _spawnCoolDown;

    public float TimeBetweenSpawns
    {
        get =>  _spawnCoolDownFloat;
        private set
        {
            _spawnCoolDown = new WaitForSeconds(value / _millisecondsToSeconds);
            _spawnCoolDownFloat = value;
        }
    }

    private void OnEnable()
    {
        _gameTimer.OnUpDifficulty += UpDifficulty;
    }
    
    private void OnDisable()
    {
        _gameTimer.OnUpDifficulty -= UpDifficulty;
    }

    [ContextMenu("BeginSpawn")]
    public void BeginSpawn()
    {
        if (_spawnerCoroutine == null)
        {
            _spawnerCoroutine = StartCoroutine(Spawner());
        }
    }

    [ContextMenu("EndSpawn")]
    public void EndSpawn()
    {
        StopCoroutine(_spawnerCoroutine);
        _spawnerCoroutine = null;
    }

    protected override void InitializeAwake()
    {
        _dropGenerator = GetComponent<AsteroidDropGenerator>();
    }

    protected override void InitializeStart()
    {
        TimeBetweenSpawns = _maxSpawnTime;
        SetTargets();
        BeginSpawn();
    }

    protected override void DisableCopy(Asteroid copy)
    {
        if (copy.IsDestroyedByPlayer)
        {
            OnAsteroidDestroyed?.Invoke(copy.transform.position, copy.ScoreAmount);
            SpawnAsteroid(copy);
        }
        
        base.DisableCopy(copy);
    }

    private void SetTargets()
    {
        foreach (Transform child in _lookAtTargetsParent)
        {
            _lookAtTargets.Add(child);
        }

        foreach (Transform child in _spawnTargetsParent)
        {
            _spawnTargets.Add(child);
        }
    }

    private IEnumerator Spawner()
    {
        while (_isSpawning)
        {
            int spawnIndex = GetRandomSpawnPlace();
            Asteroid randomAsteroid = GetRandomAsteroid();
            int targetIndex = GetRandomTarget();
            LookAtTarget(_lookAtTargets[targetIndex], _spawnTargets[spawnIndex]);
            SpawnAsteroid(randomAsteroid, spawnIndex);
            yield return _spawnCoolDown;
        }
    }

    private int GetRandomTarget()
    {
        return Random.Range(0, _lookAtTargets.Count);
    }

    private void LookAtTarget(Transform target, Transform spawnPlace)
    {
        spawnPlace.up = target.position - spawnPlace.position;
    }

    private int GetRandomSpawnPlace()
    {
        return Random.Range(0, _spawnTargets.Count);
    }

    private Asteroid GetRandomAsteroid()
    {
        if (_dropGenerator.TryGetDrop(out Asteroid asteroid))
        {
            return asteroid;
        }
        
        throw new NullReferenceException("Asteroid can not be null");
    }

    private void SpawnAsteroid(int sizeIndex, int spawnPlaceIndex)
    {
        var asteroid = EnableCopy(_spawnTargets[spawnPlaceIndex].position, _spawnTargets[spawnPlaceIndex].rotation,
            copy => (int) copy.Size == sizeIndex);
        asteroid.Enable();
    }

    private void SpawnAsteroid(Asteroid randomAsteroid, int spawnPlaceIndex)
    {
        var asteroid = EnableCopy(_spawnTargets[spawnPlaceIndex].position, _spawnTargets[spawnPlaceIndex].rotation,
            copy => copy.Size == randomAsteroid.Size);
        asteroid.Enable();
    }

    private void SpawnAsteroid(Asteroid asteroidFrom)
    {
        if (asteroidFrom.Size == Asteroid.SizeType.Small)
        {
            return;
        }

        foreach (var spawnPosition in asteroidFrom.SpawnPositions)
        {
            var asteroidRotationVector = (Vector2) spawnPosition.localPosition.normalized;
            Vector2 newDirection = asteroidFrom.Velocity.normalized + asteroidRotationVector;
            float angle = Vector2.SignedAngle(Vector2.up, newDirection);
            var asteroidRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var asteroid = EnableCopy(spawnPosition.position, asteroidRotation, copy => (int) copy.Size == (int) asteroidFrom.Size - 1);
            asteroid.Enable();
        }
    }

    private void UpDifficulty(int level)
    {
        float delta = (_maxSpawnTime - _minSpawnTime) / (float)_difficultyLevelsAmount;
        float currentSpawnTime = _maxSpawnTime - delta * level;
        
        if (currentSpawnTime < _minSpawnTime)
        {
            _gameTimer.OnUpDifficulty -= UpDifficulty;
            currentSpawnTime = _minSpawnTime;
        }
        
        TimeBetweenSpawns = currentSpawnTime;
    }
}
