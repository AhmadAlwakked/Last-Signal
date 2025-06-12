using UnityEngine;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu; // Verwijs naar het OptionsMenu Panel

    private bool isMenuOpen = false;

    void Update()
    {
        // Controleer of de Escape-toets wordt ingedrukt
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionsMenu();
        }
    }

    void ToggleOptionsMenu()
    {
        isMenuOpen = !isMenuOpen; // Wissel tussen open en gesloten
        optionsMenu.SetActive(isMenuOpen); // Toon of verberg het menu

        // Optioneel: Pauzeer de game als het menu open is
        Time.timeScale = isMenuOpen ? 0f : 1f;
    }

    // Voor een sluitknop in het optiemenu
    public void CloseOptionsMenu()
    {
        isMenuOpen = false;
        optionsMenu.SetActive(false);
        Time.timeScale = 1f; // Hervat de game
    }
}