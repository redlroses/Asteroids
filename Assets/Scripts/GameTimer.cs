using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static bool IsGlobalPause => !Convert.ToBoolean(Time.timeScale);
    
    public static event EventHandler<int> OnUpScore;
    public static event EventHandler<int> OnUpDifficulty;
    public static event EventHandler<int> OnUpSpawnChances;
    public static event EventHandler<bool> OnNextWeaponUpgrade;

    [SerializeField] private short _timeToScoreUp;
    [SerializeField] private short _timeToDifficultyUp;
    [SerializeField] private short _timeToSpawnChancesUp;
    [SerializeField] private short _timeToWeaponUpgrade;
    
    private bool _isPlay;

    private readonly List<Coroutine> _coroutines = new List<Coroutine>();

    private void Start()
    {
        SetPauseTimeScale(null, false);
        _isPlay = true;
        _coroutines.Add(StartCoroutine(Timer(_timeToDifficultyUp, OnUpDifficulty)));
        _coroutines.Add(StartCoroutine(Timer(_timeToSpawnChancesUp, OnUpSpawnChances)));
        _coroutines.Add(StartCoroutine(Timer(_timeToScoreUp, OnUpScore)));
    }

    private void ShieldSystemOnLoseGame(object sender, EventArgs e)
    {
        _isPlay = false;

        foreach (var item in _coroutines)
        {
            StopCoroutine(item);
        }
    }

    private IEnumerator Timer(int time, EventHandler<int> eventHandler)
    {
        var tick = new WaitForSeconds(time);
        int count = 0;

        do
        {
            yield return tick;
            count++;
            eventHandler?.Invoke(this, count);
        } while (_isPlay);
    }

    private IEnumerator WeaponUpgradeTimer()
    {
        var tick = new WaitForSeconds(_timeToWeaponUpgrade);
        yield return tick;
        OnNextWeaponUpgrade?.Invoke(this, true);
    }

    private void WeaponUpgradeOnWeaponUpgradeSpawns(object sender, bool e)
    {
        _coroutines.Add(StartCoroutine(WeaponUpgradeTimer()));
    }
    
    private void SetPauseTimeScale(object sender, bool flag)
    {
        Time.timeScale = Convert.ToInt32(!flag);
    }
    
    private void OnEnable()
    {
        WeaponUpgrade.OnWeaponUpgradeSpawns += WeaponUpgradeOnWeaponUpgradeSpawns;
        PlayerInputHandler.OnGamePause += SetPauseTimeScale;
        Shield.OnLoseGame += ShieldSystemOnLoseGame;
    }

    private void OnDisable()
    {
        WeaponUpgrade.OnWeaponUpgradeSpawns -= WeaponUpgradeOnWeaponUpgradeSpawns;
        PlayerInputHandler.OnGamePause -= SetPauseTimeScale;
        Shield.OnLoseGame -= ShieldSystemOnLoseGame;
    }
}