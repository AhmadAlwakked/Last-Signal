using UnityEngine;

public class TowerClick : MonoBehaviour
{
    [SerializeField] private GameObject sellCanvas; // Verwijs naar het Canvas met de verkoopknop

    void Start()
    {
        // Zorg ervoor dat het canvas standaard uit staat
        if (sellCanvas != null)
        {
            sellCanvas.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        // Controleer of het canvas is toegewezen
        if (sellCanvas != null)
        {
            // Schakel het canvas in
            sellCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Sell Canvas is niet toegewezen in de Inspector!");
        }
    }
}