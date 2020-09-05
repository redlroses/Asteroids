using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class LaserBullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _destroyTime;

    [SerializeField] private int _damage;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, _destroyTime);
        Move();
    }

    private void Move()
    {
        _rigidbody.AddRelativeForce(Vector2.up * _moveSpeed, ForceMode2D.Impulse);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Asteroid asteroid)) return;
        asteroid.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
