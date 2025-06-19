using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; } // Singleton

    [SerializeField] private GameObject[] towerPrefabs; // Array van torenprefabs, in Inspector instellen
    [SerializeField] private LayerMask groundLayer; // Laag voor raycast naar de grond
    [SerializeField] private float placementRadius = 1f; // Radius voor collider-check

    private GameObject selectedTowerPrefab;
    private GameObject previewTower;
    private bool isPlacing;
    private int currentCost;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Selecteer een toren via index (voor UI-knoppen)
    public void SelectTower(int index)
    {
        if (index < 0 || index >= towerPrefabs.Length)
        {
            Debug.LogWarning("Ongeldige torenindex!");
            return;
        }

        selectedTowerPrefab = towerPrefabs[index];
        if (selectedTowerPrefab == null)
        {
            Debug.LogWarning("Geen prefab ingesteld voor deze toren!");
            return;
        }

        TowerCost costComponent = selectedTowerPrefab.GetComponent<TowerCost>();
        if (costComponent == null)
        {
            Debug.LogWarning("Torenprefab heeft geen TowerCost-component!");
            return;
        }
        currentCost = costComponent.cost;

        // Controleer materialen
        if (!MaterialManager.Instance.SpendMaterials(currentCost))
        {
            Debug.LogWarning("Niet genoeg materialen om deze toren te bouwen!");
            return;
        }

        StartPlacement();
    }

    private void StartPlacement()
    {
        if (previewTower != null)
        {
            Destroy(previewTower);
        }

        previewTower = Instantiate(selectedTowerPrefab);
        previewTower.SetActive(false); // Uit tot cursorpositie bekend is
        isPlacing = true;
    }

    private void Update()
    {
        if (isPlacing && previewTower != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                previewTower.transform.position = hit.point;
                previewTower.SetActive(true);

                bool isValid = IsValidPosition(hit.point);
                SetPreviewColor(isValid);
            }
            else
            {
                previewTower.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0) && IsValidPosition(previewTower.transform.position))
            {
                PlaceTower();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    private bool IsValidPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, placementRadius);
        return colliders.Length == 0; // Geldige positie als er geen colliders zijn
    }

    private void SetPreviewColor(bool isValid)
    {
        Renderer renderer = previewTower.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = isValid ? Color.green : Color.red; // Groen = geldig, rood = ongeldig
        }
    }

    private void PlaceTower()
    {
        previewTower.SetActive(true);
        Renderer renderer = previewTower.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Reset naar normale kleur
        }
        selectedTowerPrefab = null;
        previewTower = null;
        isPlacing = false;
        Debug.Log("Toren geplaatst!");
    }

    private void CancelPlacement()
    {
        Destroy(previewTower);
        MaterialManager.Instance.AddMaterials(currentCost); // Refund
        selectedTowerPrefab = null;
        previewTower = null;
        isPlacing = false;
        Debug.Log("Plaatsing geannuleerd, materialen teruggegeven.");
    }
}