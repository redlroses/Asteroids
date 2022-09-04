using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AsteroidDropGenerator), typeof(AsteroidPool))]
public class AsteroidSpawner : MonoBehaviour
{
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
    private AsteroidPool _pool;
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

    private void Awake()
    {
        _pool = GetComponent<AsteroidPool>();
        _dropGenerator = GetComponent<AsteroidDropGenerator>();
    }

    private void Start()
    {
        TimeBetweenSpawns = _maxSpawnTime;
        SetTargets();
        BeginSpawn();
    }

    private void OnEnable()
    {
        _pool.AsteroidDestroyed += SpawnFromAsteroid;
        _gameTimer.UpDifficulty += UpDifficulty;
    }
    
    private void OnDisable()
    {
        _pool.AsteroidDestroyed -= SpawnFromAsteroid;
        _gameTimer.UpDifficulty -= UpDifficulty;
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

    private void SpawnAsteroid(Asteroid randomAsteroid, int spawnPlaceIndex)
    {
        var asteroid = _pool.EnableCopy(_spawnTargets[spawnPlaceIndex].position, _spawnTargets[spawnPlaceIndex].rotation,
            copy => copy.Size == randomAsteroid.Size);
        asteroid.Enable();
    }

    private void SpawnFromAsteroid(Asteroid asteroidFrom)
    {
        if (asteroidFrom.Size == Asteroid.SizeType.Small)
        {
            return;
        }

        foreach (var spawnPosition in asteroidFrom.SpawnPositions)
        {
            var asteroidRotationVector = (Vector2) spawnPosition.localPosition.normalized;
            Vector2 newDirection = asteroidFrom.Mover.Velocity.normalized + asteroidRotationVector;
            float angle = Vector2.SignedAngle(Vector2.up, newDirection);
            var asteroidRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var asteroid = _pool.EnableCopy(spawnPosition.position, asteroidRotation, copy => (int) copy.Size == (int) asteroidFrom.Size - 1);
            asteroid.Enable();
        }
    }

    private void UpDifficulty(int level)
    {
        float delta = (_maxSpawnTime - _minSpawnTime) / (float)_difficultyLevelsAmount;
        float currentSpawnTime = _maxSpawnTime - delta * level;
        
        if (currentSpawnTime < _minSpawnTime)
        {
            _gameTimer.UpDifficulty -= UpDifficulty;
            currentSpawnTime = _minSpawnTime;
        }
        
        TimeBetweenSpawns = currentSpawnTime;
    }
}
