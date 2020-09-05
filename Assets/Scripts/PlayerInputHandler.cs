using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public static event EventHandler<bool> OnGamePause;

    private bool _isPause;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPause = !_isPause;
            OnGamePause?.Invoke(this, _isPause);
        }
    }
}
