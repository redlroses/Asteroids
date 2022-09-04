using System;
using UnityEngine;

public class UiPausePanel : MonoBehaviour
{
    public event Action<bool> GamePaused;

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
        GamePaused?.Invoke(isPause);
    }
    
#if !UNITY_EDITOR
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus == false)
        {
            _isPause = true;
            SetPause(_isPause);
        }
    }
    
#endif

}