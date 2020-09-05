using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static event EventHandler<int> OnScoreUpdate;

    [SerializeField] private int _pointsPerSecond;
    [SerializeField] private int _pointsPerAsteroid;

    private int _currentScore;
    private static int[] _scoreBySize = new int [(int)Asteroid.Size.Big + 1];

    public static int ScoreBySize(int size)
    {
        return _scoreBySize[size];
    }
    
    private void Start()
    {
        for (int i = 0; i < _scoreBySize.Length; i++)
        {
            _scoreBySize[i] = Factorial(i + 1) * _pointsPerAsteroid;
        }
    }

    private void GameTimerOnUpScore(object sender, int time)
    {
        _currentScore += _pointsPerSecond;
        OnScoreUpdate?.Invoke(this, _currentScore);
    }

    private void AsteroidOnAsteroidDestroy(object sender, int size)
    {
        _currentScore += _scoreBySize[size];
        OnScoreUpdate?.Invoke(this, _currentScore);
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