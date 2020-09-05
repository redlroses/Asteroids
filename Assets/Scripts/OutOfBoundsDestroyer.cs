using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OutOfBoundsDestroyer : MonoBehaviour
{
    
    [SerializeField] private Vector2 _offset;
    
    private BoxCollider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        Vector2 bounds = PlayableFieldLimiter.GetPlayableField(false);
        bounds += _offset;
        _collider2D.size = bounds * 2;
    }
}
