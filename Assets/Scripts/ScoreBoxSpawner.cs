using Microsoft.SqlServer.Server;
using UnityEngine;

public class ScoreBoxSpawner : ObjectPool<ScoreBox>
{
    [SerializeField] private AsteroidsPool _asteroidsPool;
    
    private void OnEnable()
    {
        _asteroidsPool.OnAsteroidDestroyed += SpawnScoreBox;
    }

    private void OnDisable()
    {
        _asteroidsPool.OnAsteroidDestroyed -= SpawnScoreBox;
    }

    private void SpawnScoreBox(Vector3 position, int score)
    {
        var scoreBox = EnableCopy(position, Quaternion.identity);
        scoreBox.SetScoreText(score);
    }
}
