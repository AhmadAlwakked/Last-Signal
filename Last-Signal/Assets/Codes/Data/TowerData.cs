using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName; // Naam van de toren
    public int buildCost; // Kosten om te bouwen
    public GameObject towerPrefab; // Prefab van de toren
}