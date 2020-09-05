using System;

public class UiTimeDisplay : UiDisplay
{
    private DateTime _time;

    public void OnStartPlay()
    {
        _time = new DateTime();
        _displayText.SetText("00:00");
    }
    
    private void OnEnable()
    {
        GameTimer.OnUpScore += DisplayText;
    }

    private void OnDisable()
    {
        GameTimer.OnUpScore -= DisplayText;
    }

    protected override void DisplayText(object sender, int value)
    {
        _time = _time.AddSeconds(1);
        string timeString = _time.ToString("mm:ss");
        _displayText.SetText(timeString);
    }
    
}
