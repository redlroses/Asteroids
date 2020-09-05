using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public float TimeBetweenSpawns
    {
        get =>  _spawnCoolDownFloat;
        set
        {
            _spawnCoolDown = new WaitForSeconds(value / 1000f);
            _spawnCoolDownFloat = value;
        }
    }

    [SerializeField] private int _maxSpawnTime;
    [SerializeField] private int _minSpawnTime;
    [SerializeField] bool _isSpawning = true;
    [SerializeField] private WaitForSeconds _spawnCoolDown;
    [SerializeField] private GameObject[] _asteroidPrefabs;
    [SerializeField] private Transform _asteroidsParent;
    [SerializeField] private Transform _lookAtTargetsParent;
    [SerializeField] private Transform _spawnTargetsParent;
    [SerializeField] private int _difficultyLevelsAmount;
   
    private List<Transform> _lookAtTargets = new List<Transform>();
    private List<Transform> _spawnTargets = new List<Transform>();

    private float _spawnCoolDownFloat;
    private Coroutine _spawnerCoroutine;
    
    public void StartPlay()
    {
        _spawnerCoroutine = StartCoroutine(Spawner());
    }

    public void EndPlay()
    {
        StopCoroutine(_spawnerCoroutine);
    }
    
    private void Start()
    {
        TimeBetweenSpawns = _maxSpawnTime;
        SetSpawnAndLookAtTargets();
        StartPlay();
    }

    private void SetSpawnAndLookAtTargets()
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
            int sizeIndex = GetRandomAsteroidSize();
            int targetIndex = GetRandomTarget();
            LookAtTarget(_lookAtTargets[targetIndex], _spawnTargets[spawnIndex]);
            SpawnAsteroid(sizeIndex, spawnIndex);
            yield return _spawnCoolDown;
        }
    }

    private int GetRandomTarget()
    {
        int randomTargetIndex = Random.Range(0, _lookAtTargets.Count);
        return randomTargetIndex;
    }

    private void LookAtTarget(Transform target, Transform spawnPlace)
    {
        Vector3 direction = target.position - spawnPlace.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        spawnPlace.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private int GetRandomSpawnPlace()
    {
        int randomSpawnPlace = Random.Range(0, _spawnTargets.Count);
        return randomSpawnPlace;
    }

    private int GetRandomAsteroidSize()
    {
        int randomSize = Randomizer.GetRandomAsteroidId();
        return randomSize;
    }

    private void SpawnAsteroid(int sizeIndex, int spawnPlaceIndex)
    {
        Instantiate(_asteroidPrefabs[sizeIndex], _spawnTargets[spawnPlaceIndex].position, _spawnTargets[spawnPlaceIndex].rotation, _asteroidsParent);
    }

    private void UpDifficulty(object sender, int level)
    {
        float currentSpawnTime = TimeBetweenSpawns;
        
        if (currentSpawnTime <= _minSpawnTime)  GameTimer.OnUpDifficulty -= UpDifficulty;
        
        float delta = (_maxSpawnTime - _minSpawnTime) / (float)_difficultyLevelsAmount;
        currentSpawnTime = _maxSpawnTime - delta * level;
        
        if (currentSpawnTime < _minSpawnTime)
        {
            currentSpawnTime = _minSpawnTime;
        }
        //Debug.Log($"Увеличилась сложность. Уровень: {level}. Время спавна астероида: {currentSpawnTime}");
        TimeBetweenSpawns = currentSpawnTime;
    }
    
    private void OnEnable()
    {
        GameTimer.OnUpDifficulty += UpDifficulty;
    }
    
    private void OnDisable()
    {
        GameTimer.OnUpDifficulty -= UpDifficulty;
    }
}
