using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class AsteroidScore : MonoBehaviour
{
    private TextMeshPro _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    public void SetScoreText(int score)
    {
        _text.SetText($"+{score}");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}