using System;
using UnityEngine;

public class UiTimeDisplayText : UiDisplayText
{
    private readonly int _oneSecond = 1;
    
    [SerializeField] private GameTimer _gameTimer;
    
    private DateTime _time;

    private void OnEnable()
    {
        _gameTimer.UpScoreByTime += DisplayText;
    }

    private void OnDisable()
    {
        _gameTimer.UpScoreByTime -= DisplayText;
    }

    public void StartPlay()
    {
        _time = new DateTime();
        Text.SetText("00:00");
    }

    public override void DisplayText(int value)
    {
        _time = _time.AddSeconds(_oneSecond);
        string timeString = _time.ToString("mm:ss");
        Text.SetText(timeString);
    }
}
