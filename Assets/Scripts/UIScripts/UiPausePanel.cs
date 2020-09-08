using System;
using UnityEngine;

public class UiPausePanel : MonoBehaviour
{
    public static event EventHandler<bool> OnGamePause;

    [SerializeField] private GameObject _pausePanel;

    private bool _isPause;

    public void SwitchPause()
    {
        _isPause = !_isPause;
        SetPause(_isPause);
    }

    private void SetPause(bool isPause)
    {
        _pausePanel.SetActive(isPause);
        OnGamePause?.Invoke(this, isPause);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus == false)
        {
            _isPause = true;
            SetPause(_isPause);
        }
    }
}