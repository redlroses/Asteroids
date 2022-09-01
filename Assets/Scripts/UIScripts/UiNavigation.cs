using UnityEngine;
using UnityEngine.SceneManagement;

public class UiNavigation : MonoBehaviour
{
    [SerializeField] private GameTimer _gameTimer;
    
    public void ToMenu()
    {
        _gameTimer.ResetGame();
        SceneManager.LoadScene("Scenes/Menu");
    }
}
