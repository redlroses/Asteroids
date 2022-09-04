using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public event Action<int> ScoreUpdate;
    public event Action NewRecord;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private DataContainer _dataContainer;
    [SerializeField] private AsteroidPool _asteroidPool;
    [SerializeField] private int _pointsPerSecond;
    [SerializeField] private int _currentScore;

    private bool _isNewRecordWas;
    private int _highScoreRecord;

    public int CurrentScore
    {
        get => _currentScore;

        private set
        {
            _currentScore = value;
            ScoreUpdate?.Invoke(_currentScore);
            
            if (_isNewRecordWas || _currentScore <= _highScoreRecord) return;

            _isNewRecordWas = true;
            NewRecord?.Invoke();
        }
    }

    private void Start()
    {
        _highScoreRecord = _dataContainer.GetHighRecord();
    }

    public void SaveScore()
    {
        _dataContainer.AddRecord(_currentScore);
    }

    private void IncreaseScoreByTime(int time)
    {
        CurrentScore += _pointsPerSecond;
    }

    private void IncreaseScoreByAsteroid(Asteroid asteroid)
    {
        CurrentScore += asteroid.ScoreAmount;
    }

    private void OnEnable()
    {
        _gameTimer.UpScoreByTime += IncreaseScoreByTime;
        _asteroidPool.AsteroidDestroyed += IncreaseScoreByAsteroid;
    }

    private void OnDisable()
    {
        _gameTimer.UpScoreByTime -= IncreaseScoreByTime;
        _asteroidPool.AsteroidDestroyed -= IncreaseScoreByAsteroid;
    }

    private void OnDestroy()
    {
        SaveScore();
    }
}