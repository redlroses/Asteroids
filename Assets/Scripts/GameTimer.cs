using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    private readonly List<Coroutine> _coroutines = new List<Coroutine>();
    
    public event Action<int> OnUpScoreByTime;
    public event Action<int> OnUpDifficulty;
    public event Action<int> OnUpSpawnChances;

    [SerializeField] private short _timeToScoreUp;
    [SerializeField] private short _timeToDifficultyUp;
    [SerializeField] private short _timeToSpawnChancesUp;
    [SerializeField] private UiPausePanel _uiPausePanel;
    [SerializeField] private Shield _shield;
    [SerializeField] private UnityEvent _onEndGame;

    private bool _isPlay;

    private void Start()
    {
        SetPause(false);
        _isPlay = true;
        _coroutines.Add(StartCoroutine(Timer(_timeToDifficultyUp, OnUpDifficulty)));
        _coroutines.Add(StartCoroutine(Timer(_timeToSpawnChancesUp, OnUpSpawnChances)));
        _coroutines.Add(StartCoroutine(Timer(_timeToScoreUp, OnUpScoreByTime)));
    }

    public void PauseGame()
    {
        SetPause(true);
    }
    public void ResetGame()
    {
        SetPause(false);
    }
    
    private void StopCoroutines()
    {
        _isPlay = false;

        foreach (var coroutine in _coroutines)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator Timer(int time, Action<int> eventHandler)
    {
        var tick = new WaitForSeconds(time);
        int count = 0;

        do 
        {
            yield return tick;
            count++;
            eventHandler?.Invoke(count);
        } 
        while (_isPlay);
    }

    private void SetPause(bool isPause)
    {
        Time.timeScale = Convert.ToInt32(!isPause);
    }

    private void EndGame()
    {
        StopCoroutines();
        _onEndGame?.Invoke();
    }
    
    private void OnEnable()
    {
        _uiPausePanel.OnGamePaused += SetPause;
        _shield.OnShipDead += EndGame;
    }

    private void OnDisable()
    {
        _uiPausePanel.OnGamePaused -= SetPause;
        _shield.OnShipDead -= EndGame;
    }
}