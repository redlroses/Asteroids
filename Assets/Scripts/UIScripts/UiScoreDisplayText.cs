using UnityEngine;

public class UiScoreDisplayText : UiDisplayText
{
    [SerializeField] private ScoreCounter _scoreCounter;
    
    private void OnEnable()
    {
        _scoreCounter.ScoreUpdate += DisplayText;
    }
    
    private void OnDisable()
    {
        _scoreCounter.ScoreUpdate -= DisplayText;
    }
}