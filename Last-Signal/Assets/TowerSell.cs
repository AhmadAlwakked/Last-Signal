using UnityEngine;
using UnityEngine.UI;

public class TowerSell : MonoBehaviour
{
    [SerializeField] private GameObject sellUIPanel; // Sleep SellUI hier
    [SerializeField] private Button sellButton;      // Sleep SellButton hier

    void Start()
    {
        // Controleer of alles is gekoppeld
        if (sellUIPanel == null || sellButton == null)
        {
           
        }
        sellUIPanel.SetActive(false); // UI begint uit
        sellButton.onClick.AddListener(SellTower); // Koppel knop aan functie
    }

    void OnMouseDown()
    {
        Debug.Log("Geklikt op toren: " + gameObject.name);
        sellUIPanel.SetActive(true); // Toon UI
    }

    void SellTower()
    {
        Debug.Log("Toren verkocht!");
        sellUIPanel.SetActive(false); // Verberg UI
        Destroy(gameObject); // Verwijder toren
    }
}