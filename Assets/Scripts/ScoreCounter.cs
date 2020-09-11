using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static event EventHandler<int> OnScoreUpdate;
    public static event EventHandler OnNewRecord;

    public int CurrentScore
    {
        get => _currentScore;

        private set
        {
            _currentScore = value;
            OnScoreUpdate?.Invoke(this, _currentScore);
            
            if (_isNewRecordWas || _currentScore <= _highScoreRecord) return;

            _isNewRecordWas = true;
            OnNewRecord?.Invoke(this, EventArgs.Empty);
        }
    }

    [SerializeField] private int _pointsPerSecond;
    [SerializeField] private int _pointsPerAsteroid;

    private bool _isNewRecordWas;
    private int _highScoreRecord;
    private int _currentScore;
    private static int[] _pointsBySize = new int [(int) Asteroid.Size.Big + 1];

    /// <summary>
    /// Возвращает очки за размер астероида с данным индексом
    /// </summary>
    /// <param name="size"></param>
    /// <returns>score</returns>
    public static int PointsBySize(int size)
    {
        return _pointsBySize[size];
    }

    // Вызывается событием Юнити при выходе в меню из панели паузы или панели проигрыша 
    public void SaveScore()
    {
        DataSaver.AddRecord(_currentScore);
    }

    private void Start()
    {
        _highScoreRecord = DataSaver.GetHighRecord();

        // Инициализация контейнера очков
        // Скейл очков на базе факториала 
        for (int i = 0; i < _pointsBySize.Length; i++)
        {
            _pointsBySize[i] = Factorial(i + 1) * _pointsPerAsteroid;
        }
    }

    private void GameTimerOnUpScore(object sender, int time)
    {
        CurrentScore += _pointsPerSecond;
    }

    private void AsteroidOnAsteroidDestroy(object sender, int size)
    {
        CurrentScore += _pointsBySize[size];
    }

    private int Factorial(int value)
    {
        if (value == 0)
        {
            return 1;
        }

        return value * Factorial(value - 1);
    }

    private void OnEnable()
    {
        GameTimer.OnUpScore += GameTimerOnUpScore;
        Asteroid.OnAsteroidDestroy += AsteroidOnAsteroidDestroy;
    }

    private void OnDisable()
    {
        GameTimer.OnUpScore -= GameTimerOnUpScore;
        Asteroid.OnAsteroidDestroy -= AsteroidOnAsteroidDestroy;
    }
}