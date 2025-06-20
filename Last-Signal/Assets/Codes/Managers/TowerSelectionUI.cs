using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    [System.Serializable]
    public struct TowerButtonConfig
    {
        public Button button; // De UI-knop
        public GameObject towerPrefab; // De torenprefab
        public int cost; // De kosten van de toren
    }

    [SerializeField] private TowerButtonConfig[] towerButtons = new TowerButtonConfig[4]; // Array voor 4 knoppen

    private void Start()
    {
        // Controleer of alle knoppen en prefabs zijn ingesteld
        for (int i = 0; i < towerButtons.Length; i++)
        {
            if (towerButtons[i].button == null)
            {
                Debug.LogError($"Knop {i} is niet ingesteld in TowerSelectionUI!");
                continue;
            }
            if (towerButtons[i].towerPrefab == null)
            {
                Debug.LogError($"Torenprefab voor knop {i} is niet ingesteld in TowerSelectionUI!");
                continue;
            }
            if (towerButtons[i].cost <= 0)
            {
                Debug.LogWarning($"Kosten voor knop {i} zijn ongeldig ({towerButtons[i].cost}), default naar 100.");
                towerButtons[i].cost = 100;
            }

            // Koppel de knop aan de BuildManager met debug-logging
            int index = i; // Lokale kopie voor closure
            towerButtons[i].button.onClick.AddListener(() =>
            {
                Debug.Log($"Knop {index} geklikt met prefab {towerButtons[index].towerPrefab.name} en kosten {towerButtons[index].cost}");
                BuildManager.Instance.SelectTower(towerButtons[index].towerPrefab, towerButtons[index].cost);
            });
        }
    }
}