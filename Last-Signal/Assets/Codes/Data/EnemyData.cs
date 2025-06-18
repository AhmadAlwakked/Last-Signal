using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Tower Defense/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName; // Naam van de vijand
    public int rewardMaterials; // Materialen bij dood
}