using UnityEngine;

public class UiScoreDisplayText : UiDisplayText
{
    [SerializeField] private ScoreCounter _scoreCounter;
    
    private void OnEnable()
    {
        _scoreCounter.OnScoreUpdate += DisplayText;
    }
    
    private void OnDisable()
    {
        _scoreCounter.OnScoreUpdate -= DisplayText;
    }
}