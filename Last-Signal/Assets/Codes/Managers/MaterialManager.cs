using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance { get; private set; } // Singleton
    [SerializeField] private int materials = 100; // Startaantal materialen, aanpasbaar in Inspector

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Voorkom duplicaten
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Behoud tussen scenes
    }

    public int GetMaterials()
    {
        return materials;
    }

    public void AddMaterials(int amount)
    {
        if (amount > 0)
        {
            materials += amount;
            Debug.Log($"Materialen toegevoegd: {amount}. Totaal: {materials}");
        }
        else
        {
            Debug.LogWarning("Kan geen negatieve of nul materialen toevoegen!");
        }
    }

    public bool SpendMaterials(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Kan geen negatieve of nul materialen uitgeven!");
            return false;
        }
        if (materials >= amount)
        {
            materials -= amount;
            Debug.Log($"Materialen uitgegeven: {amount}. Totaal: {materials}");
            return true;
        }
        else
        {
            Debug.LogWarning("Niet genoeg materialen!");
            return false;
        }
    }

    // Voor testen: voeg materialen toe via een toets
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // Druk op 'M' om te testen
        {
            AddMaterials(10);
        }
    }
}