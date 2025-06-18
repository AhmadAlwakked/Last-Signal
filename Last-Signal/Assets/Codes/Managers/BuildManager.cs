using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; } // Singleton
    private TowerData selectedTower; // Geselecteerde toren om te bouwen

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SelectTower(TowerData towerData)
    {
        selectedTower = towerData;
        Debug.Log($"Geselecteerde toren: {towerData.towerName}");
    }

    public bool CanBuild(Vector3 position)
    {
        if (selectedTower == null)
        {
            Debug.LogWarning("Geen toren geselecteerd!");
            return false;
        }
        // Controleer of de positie vrij is (simpele check)
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        bool isPositionFree = colliders.Length == 0;
        if (!isPositionFree)
        {
            Debug.LogWarning("Positie is bezet!");
            return false;
        }
        // Controleer of er genoeg materialen zijn
        return MaterialManager.Instance.SpendMaterials(selectedTower.buildCost);
    }

    public void BuildTower(Vector3 position)
    {
        if (CanBuild(position))
        {
            GameObject tower = Instantiate(selectedTower.towerPrefab, position, Quaternion.identity);
            Debug.Log($"Toren gebouwd: {selectedTower.towerName} op {position}");
            selectedTower = null; // Reset selectie
        }
    }

    // Voor testen: bouw een toren met een toets
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && selectedTower != null) // Druk op 'B' om te testen
        {
            BuildTower(new Vector3(0, 0, 0)); // Testpositie
        }
    }
}