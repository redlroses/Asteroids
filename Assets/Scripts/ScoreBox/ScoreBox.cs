using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class ScoreBox : MonoBehaviour, IPoolable<ScoreBox>
{
    public event Action<ScoreBox> Disabled;
    
    [SerializeField] private TextMeshPro _text;

    public void SetScoreText(int score)
    {
        _text.SetText($"+{score}");
    }

    public void Destroy()
    {
        Disabled?.Invoke(this);
    }

    public bool GetActiveSelf()
    {
        return gameObject.activeSelf;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}