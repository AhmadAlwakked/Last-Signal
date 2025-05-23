using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    // Functie om het spel te starten
    public void StartTheGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void StartTheCredits()
    {
        SceneManager.LoadScene("Creaters Menu");
    }
    // Functie om terug te keren naar het hoofdmenu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Zorg dat "Main Menu" exact de naam van je scène is
    }

    // Functie om de game af te sluiten
    public void QuitGame()
    {
        Debug.Log("Game is quitting..."); // Log om te bevestigen dat de functie werkt
        Application.Quit(); // Sluit de applicatie

        // Voor testen in de Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}