using UnityEngine;

public class BonusMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;

    private void Update()
    {
        transform.Translate(Vector3.down * (Time.deltaTime * _moveSpeed), Space.World);
    }
}
