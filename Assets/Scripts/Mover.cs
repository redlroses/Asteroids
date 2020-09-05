using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : Player
{
    public static event EventHandler OnMaxPlayerSpeed;

    public float DefaultMoveSpeed => _defaultMoveSpeed;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            _moveSpeed = value;

            if (_moveSpeed >= _maxMoveSpeed)
            {
                _moveSpeed = _maxMoveSpeed;
                OnMaxPlayerSpeed?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    [SerializeField] private float _defaultMoveSpeed = 10f;
    [SerializeField] private float _maxMoveSpeed = 15;


    private float _moveSpeed = default;

    private Vector2 _moveDirection = default;
    private Vector2 _playablefield = default;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        _moveSpeed = _defaultMoveSpeed;
        _playablefield = PlayableFieldLimiter.GetPlayableField(true);
    }

    private void Update()
    {
        Move(_moveDirection);
    }

    private void Move(Vector2 direction)
    {
        float scaledMoveSpeed = _moveSpeed * Time.deltaTime;

        Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);

        Vector3 position = transform.position;
        position += scaledMoveSpeed * moveDirection;
        position = ClampVector2(position, _playablefield.x, _playablefield.y);
        transform.position = position;
    }

    private Vector2 ClampVector2(Vector2 currentVector, float horizontalLimit, float verticalLimit)
    {
        float clampedHorizontal = Mathf.Clamp(currentVector.x, -horizontalLimit, horizontalLimit);
        float clampedVertical = Mathf.Clamp(currentVector.y, -verticalLimit, verticalLimit);

        return new Vector2(clampedHorizontal, clampedVertical);
    }
}