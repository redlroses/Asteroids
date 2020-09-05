public class UiScoreDisplay : UiDisplay
{
    private void OnEnable()
    {
        ScoreCounter.OnScoreUpdate += DisplayText;
    }
    
    private void OnDisable()
    {
        ScoreCounter.OnScoreUpdate -= DisplayText;
    }
}