using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public Camera mainCamera;
    private GameObject selectedTowerPrefab;
    private GameObject previewTower;

    public LayerMask placementLayer;

    public void SelectTower(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;

        if (previewTower != null)
            Destroy(previewTower);

        previewTower = Instantiate(towerPrefab);
        previewTower.GetComponent<Collider>().enabled = false; // optioneel
        SetPreviewMaterial(previewTower); // transparant maken
    }

    void Update()
    {
        if (selectedTowerPrefab == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementLayer))
        {
            if (previewTower != null)
                previewTower.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0)) // linkerklik = plaatsen
            {
                Instantiate(selectedTowerPrefab, hit.point, Quaternion.identity);
                Destroy(previewTower);
                selectedTowerPrefab = null;
            }
        }
    }

    void SetPreviewMaterial(GameObject obj)
    {
        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (Material m in r.materials)
            {
                m.shader = Shader.Find("Transparent/Diffuse");
                Color c = m.color;
                c.a = 0.5f;
                m.color = c;
            }
        }
    }
}
