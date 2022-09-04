using UnityEngine;

[RequireComponent(typeof(Animation))]
public class UiNewRecord : MonoBehaviour
{
    [SerializeField] private ScoreCounter _scoreCounter;
    
    private Animation _animation;

    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        _scoreCounter.NewRecord += StartAnimation;
    }

    private void OnDisable()
    {
        _scoreCounter.NewRecord -= StartAnimation;
    }

    private void StartAnimation()
    {
        _animation.Play();
    }
}