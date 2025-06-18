using UnityEngine;

public class TestTowerSelector : MonoBehaviour
{
    [SerializeField] private TowerData towerData; // Sleep TowerData hier naartoe in Inspector

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Druk op 'T' om te testen
        {
            BuildManager.Instance.SelectTower(towerData);
        }
    }
}