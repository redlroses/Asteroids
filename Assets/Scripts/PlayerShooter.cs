using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    public static event EventHandler OnStartShoot;
    public static event EventHandler OnFinishShoot;

    public void OnShoot(InputAction.CallbackContext context)
    {
        // IsGlobalPause позволяет не стрелять сразу после отжатия паузы
        if (context.started & !GameTimer.IsGlobalPause) 
        {
            OnStartShoot?.Invoke(this, EventArgs.Empty);
        }
        if (context.canceled)
        {
            OnFinishShoot?.Invoke(this, EventArgs.Empty);
        }
    }
}
