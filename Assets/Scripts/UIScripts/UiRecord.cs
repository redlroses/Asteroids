using TMPro;
using UnityEngine;

public class UiRecord : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _positionText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void SetData(int position, int score)
    {
        _positionText.SetText(position.ToString());
        _scoreText.SetText(score.ToString());
    }
}
