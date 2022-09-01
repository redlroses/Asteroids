using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _defaultMoveSpeed = 10f;
    [SerializeField] private float _maxMoveSpeed = 15;

    private float _moveSpeed;
    private Vector2 _moveDirection;
    private Vector2 _playablefield;
    private Vector2 _delta;
    
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
            }
        }
    }

    private void Start()
    {
        _moveSpeed = _defaultMoveSpeed;
        _playablefield = PlayableFieldLimiter.GetPlayableField(true);
    }

    public void IncreaseSpeed(float value)
    {
        MoveSpeed += Mathf.Clamp(value, 0, float.MaxValue);
    }

    public void Move(Vector2 direction) 
    {
        float scaledSpeed = _moveSpeed * Time.deltaTime;
        direction = GetClampedVector2(direction, _playablefield.x, _playablefield.y);
        transform.position = Vector2.MoveTowards(transform.position, direction, scaledSpeed);
    }

    private Vector2 GetClampedVector2(Vector2 vector, float horizontalLimit, float verticalLimit)
    {
        float horizontal = Mathf.Clamp(vector.x, -horizontalLimit, horizontalLimit);
        float vertical = Mathf.Clamp(vector.y, -verticalLimit, verticalLimit);
        
        return new Vector2(horizontal, vertical);
    }
}