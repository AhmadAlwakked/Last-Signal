using UnityEngine;
using TMPro;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance { get; private set; }
    [SerializeField] private int materials = 100;
    [SerializeField] private TMP_Text materialsText; // Sleep TMP Text hierheen in de Inspector

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateMaterialText();
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
            UpdateMaterialText();
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
            UpdateMaterialText();
            return true;
        }
        else
        {
            Debug.LogWarning("Niet genoeg materialen!");
            return false;
        }
    }

    private void UpdateMaterialText()
    {
        if (materialsText != null)
        {
            materialsText.text = $"Materialen: {materials}";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMaterials(10);
        }
    }
}