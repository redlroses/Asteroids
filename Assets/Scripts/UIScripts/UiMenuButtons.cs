using UnityEngine;
using UnityEngine.SceneManagement;


public class UiMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;

    public void OnPlay()
    {
        SceneManager.LoadScene("Scenes/Main");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}