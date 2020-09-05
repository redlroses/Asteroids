using UnityEngine;
using UnityEngine.SceneManagement;

public class UiNavigation : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
