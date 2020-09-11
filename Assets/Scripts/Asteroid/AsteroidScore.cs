using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class AsteroidScore : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    
    public void SetScoreText(int score)
    {
        _text.SetText($"+{score}");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}