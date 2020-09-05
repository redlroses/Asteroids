using UnityEngine;

public class UiPausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    private void PlayerInputHandlerOnGamePause(object sender, bool flag)
    {
        _pausePanel.SetActive(flag);
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnGamePause += PlayerInputHandlerOnGamePause;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnGamePause -= PlayerInputHandlerOnGamePause;
    }
}