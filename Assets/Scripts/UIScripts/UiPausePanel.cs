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
        _pausePanel.SetActive(_isPause);
        OnGamePause?.Invoke(this, _isPause);
    }
}