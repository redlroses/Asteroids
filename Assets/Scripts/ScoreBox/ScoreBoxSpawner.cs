using UnityEngine;

[RequireComponent(typeof(ScoreBoxPool))]
public class ScoreBoxSpawner : MonoBehaviour
{
    [SerializeField] private AsteroidPool _asteroidPool;

    private ScoreBoxPool _pool;

    private void Awake()
    {
        _pool = GetComponent<ScoreBoxPool>();
    }
    
    private void OnEnable()
    {
        _asteroidPool.AsteroidDestroyed += SpawnScoreBox;
    }

    private void OnDisable()
    {
        _asteroidPool.AsteroidDestroyed -= SpawnScoreBox;
    }

    private void SpawnScoreBox(Asteroid asteroid)
    {
        var scoreBox = _pool.EnableCopy(asteroid.transform.position, Quaternion.identity);
        scoreBox.SetScoreText(asteroid.ScoreAmount);
    }
}
