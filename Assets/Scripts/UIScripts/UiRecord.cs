using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiRecord : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _positionText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    /// <summary>
    /// Set information of position on leaderboard and corresponding score
    /// </summary>
    /// <param name="position"></param>
    /// <param name="score"></param>
    public void SetData(int position, int score)
    {
        _positionText.SetText(position.ToString());
        _scoreText.SetText(score.ToString());
    }
}
