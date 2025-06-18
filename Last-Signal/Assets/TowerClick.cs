using UnityEngine;
using UnityEngine.EventSystems;

public class TowerClick : MonoBehaviour
{
    [SerializeField] private GameObject sellCanvas; // Verwijs naar het Canvas met de verkoopknop

    private void Start()
    {
        // Zorg ervoor dat het canvas standaard uit staat
        if (sellCanvas != null)
        {
            sellCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Sell Canvas is niet toegewezen in de Inspector!");
        }
    }

    private void OnMouseDown()
    {
        // Controleer of het canvas is toegewezen en of de klik niet op een UI-element is
        if (sellCanvas != null && !EventSystem.current.IsPointerOverGameObject())
        {
            // Schakel het canvas in
            sellCanvas.SetActive(true);
        }
    }

    private void Update()
    {
        // Controleer of het canvas actief is en of er een muisklik is
        if (sellCanvas != null && sellCanvas.activeSelf && Input.GetMouseButtonDown(0))
        {
            // Controleer of de klik buiten het canvas en de toren is
            if (!EventSystem.current.IsPointerOverGameObject() && !IsClickOnTower())
            {
                sellCanvas.SetActive(false);
            }
        }
    }

    private bool IsClickOnTower()
    {
        // Converteer muispositie naar een ray in de wereld
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Controleer of de ray de toren raakt
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }

        return false;
    }
}