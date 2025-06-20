using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; } // Singleton

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

    // Selecteer een toren (aangeroepen door TowerSelectionUI)
    public void SelectTower(GameObject towerPrefab, int cost)
    {
        Debug.Log($"SelectTower aangeroepen met prefab {towerPrefab.name} en kosten {cost}");
        if (towerPrefab == null)
        {
            Debug.LogWarning("Geen torenprefab geselecteerd!");
            return;
        }
        if (cost <= 0)
        {
            Debug.LogWarning($"Ongeldige kosten ({cost}), default naar 100.");
            cost = 100;
        }

        // Controleer materialen
        if (!MaterialManager.Instance.SpendMaterials(cost))
        {
            Debug.LogWarning("Niet genoeg materialen om deze toren te bouwen!");
            return;
        }

        selectedTowerPrefab = towerPrefab;
        currentCost = cost;
        StartPlacement();
    }

    private void StartPlacement()
    {
        Debug.Log("StartPlacement aangeroepen");
        if (previewTower != null)
        {
            Destroy(previewTower);
        }

        previewTower = Instantiate(selectedTowerPrefab);
        if (previewTower == null)
        {
            Debug.LogError("Preview-toren kon niet worden geïnstantieerd!");
            return;
        }
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
        else
        {
            Debug.LogWarning("Geen Renderer gevonden in preview-toren!");
        }
    }

    private void PlaceTower()
    {
        Debug.Log("Toren geplaatst");
        previewTower.SetActive(true);
        Renderer renderer = previewTower.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Reset naar normale kleur
        }
        selectedTowerPrefab = null;
        previewTower = null;
        isPlacing = false;
    }

    private void CancelPlacement()
    {
        Debug.Log("Plaatsing geannuleerd");
        Destroy(previewTower);
        MaterialManager.Instance.AddMaterials(currentCost); // Refund
        selectedTowerPrefab = null;
        previewTower = null;
        isPlacing = false;
    }
}