using UnityEngine;

public class Debugger : MonoBehaviour
{
    private void OnEnable()
    {
        //ScoreCounter.OnScoreUpdate += ScoreCounterOnScoreUpdate;
    }

    private void ScoreCounterOnScoreUpdate(object sender, int score)
    {
        Debug.Log($"Очков: {score}");
    }
}
