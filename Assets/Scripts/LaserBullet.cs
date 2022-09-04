using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class LaserBullet : MonoBehaviour, IPoolable<LaserBullet>
{
    public event Action<LaserBullet> Disabled;
    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _damage;

    private Rigidbody2D _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            asteroid.TakeDamage(_damage);
        }
        
        Disabled?.Invoke(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeSelf && other.TryGetComponent(out OutOfBoundsDestroyer destroyer))
        {
            Disabled?.Invoke(this);
        }
    }

    private void Move()
    {
        _rigidbody.AddRelativeForce(Vector2.up * _moveSpeed, ForceMode2D.Impulse);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
