using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidMover : MonoBehaviour
{
    [SerializeField] private Vector2 _moveSpeedRange;
    [SerializeField] private Vector2 _rotationSpeedRange;
    
    private Rigidbody2D _rigidbody;
    
    public Vector2 Velocity => _rigidbody.velocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.AddRelativeForce(Vector2.up * Random.Range(_moveSpeedRange.x, _moveSpeedRange.y),
            ForceMode2D.Impulse);
        _rigidbody.AddTorque(Random.Range(_rotationSpeedRange.x, _rotationSpeedRange.y), ForceMode2D.Impulse);
    }
}
