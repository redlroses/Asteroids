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

    public void OnSettings()
    {
        _menuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void OnMenuFromSettings()
    {
        _menuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}