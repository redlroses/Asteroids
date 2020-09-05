using System;
using UnityEngine;
using Input = InputWrapper.Input;

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

    private float _moveSpeed;
    private Camera _camera;
    
    private Vector2 _moveDirection;
    private Vector2 _playablefield ;
    private Vector2 _delta;

    private void Start()
    {
        _camera = Camera.main;
        _moveSpeed = _defaultMoveSpeed;
        _playablefield = PlayableFieldLimiter.GetPlayableField(true);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector2 direction = _camera.ScreenToWorldPoint(Input.touches[0].position);
            
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                _delta = (Vector2)transform.position - direction;
            }
         
            Move(direction + _delta);
        }
    }

    private void Move(Vector2 direction)
    {
        var scaledSpeed = _moveSpeed * Time.deltaTime;
        var clampedDirection = ClampVector2(direction, _playablefield.x, _playablefield.y);
        transform.position = Vector2.MoveTowards(transform.position, clampedDirection, scaledSpeed);
    }
    
    // private void Move(Vector2 direction)
    // {
    //     float scaledMoveSpeed = _moveSpeed * Time.deltaTime;
    //
    //     Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);
    //
    //     Vector3 position = transform.position;
    //     position += scaledMoveSpeed * moveDirection;
    //     position = ClampVector2(position, _playablefield.x, _playablefield.y);
    //     transform.position = position;
    // }

    private Vector2 ClampVector2(Vector2 currentVector, float horizontalLimit, float verticalLimit)
    {
        float clampedHorizontal = Mathf.Clamp(currentVector.x, -horizontalLimit, horizontalLimit);
        float clampedVertical = Mathf.Clamp(currentVector.y, -verticalLimit, verticalLimit);
    
        return new Vector2(clampedHorizontal, clampedVertical);
    }
}