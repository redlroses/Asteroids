using System;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class UiNewRecord : MonoBehaviour
{
    private Animation _animation;

    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    private void StartAnimation(object sender, EventArgs e)
    {
        _animation.Play();
    }

    private void OnEnable()
    {
        ScoreCounter.OnNewRecord += StartAnimation;
    }

    private void OnDisable()
    {
        ScoreCounter.OnNewRecord -= StartAnimation;
    }
}