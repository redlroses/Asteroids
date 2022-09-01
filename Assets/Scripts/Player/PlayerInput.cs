using UnityEngine;
using Input = InputWrapper.Input;

[RequireComponent(typeof(Mover))]
public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    private Mover _mover;
    private Vector2 _delta;

    private void Start()
    {
        _camera = Camera.main;
        _mover = GetComponent<Mover>();
    }

    private void Update()
    {
        CatchInput();
    }

    private void CatchInput()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }
        
        Vector2 touchPosition = _camera.ScreenToWorldPoint(Input.touches[0].position);
            
        if (Input.touches[0].phase == TouchPhase.Began)
        {
            _delta = (Vector2)transform.position - touchPosition;
        }

        _mover.Move(touchPosition + _delta);
    }
}
