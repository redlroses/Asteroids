using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public event Action<int> OnScoreUpdate;
    public event Action OnNewRecord;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private DataContainer _dataContainer;
    [SerializeField] private AsteroidsPool _asteroidsPool;
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
            OnScoreUpdate?.Invoke(_currentScore);
            
            if (_isNewRecordWas || _currentScore <= _highScoreRecord) return;

            _isNewRecordWas = true;
            OnNewRecord?.Invoke();
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

    private void IncreaseScoreByAsteroid(Vector3 position, int score)
    {
        CurrentScore += score;
    }

    private void OnEnable()
    {
        _gameTimer.OnUpScoreByTime += IncreaseScoreByTime;
        _asteroidsPool.OnAsteroidDestroyed += IncreaseScoreByAsteroid;
    }

    private void OnDisable()
    {
        _gameTimer.OnUpScoreByTime -= IncreaseScoreByTime;
        _asteroidsPool.OnAsteroidDestroyed -= IncreaseScoreByAsteroid;
    }

    private void OnDestroy()
    {
        SaveScore();
    }
}