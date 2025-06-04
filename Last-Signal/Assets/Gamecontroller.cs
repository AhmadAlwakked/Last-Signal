using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Laadt de spelsc�ne
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Laadt de settings-sc�ne
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");

    }

    public void Return()
    {
        SceneManager.LoadScene("Game");

    }

    // Sluit het spel
    public void ExitGame()
    {
        Application.Quit();
    }
}