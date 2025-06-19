using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private float waveInterval = 30f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> allEnemyTypes = new List<GameObject>(); // Prefabs uit Assets
    [SerializeField] private Transform[] waypoints; // Empty objects als waypoints, ingesteld in Inspector
    [SerializeField] private int enemiesPerTypePerWave = 3; // Vaste aantal vijanden per type per golf
    [SerializeField] private float spawnDelay = 1f;

    private float nextWaveTime;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaveIndex = 0;
    private List<GameObject> activeEnemyTypes = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Debug.Log("WaveManager geactiveerd. Aantal enemy types: " + allEnemyTypes.Count);
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Geen waypoints ingesteld in WaveManager! Voeg empty objects toe aan waypoints.");
        }
        if (spawnPoint == null)
        {
            Debug.LogError("Geen spawnPoint ingesteld!");
        }
    }

    private void Start()
    {
        nextWaveTime = Time.time + waveInterval;
        if (allEnemyTypes.Count >= 2)
        {
            activeEnemyTypes.Add(allEnemyTypes[0]);
            activeEnemyTypes.Add(allEnemyTypes[1]);
            Debug.Log("Actieve enemy types ingesteld: " + activeEnemyTypes.Count);
        }
        else
        {
            Debug.LogWarning("Te weinig enemy types! Minimaal 2 vereist.");
        }
    }

    private void Update()
    {
        if (Time.time >= nextWaveTime && spawnPoint != null && activeEnemyTypes.Count > 0)
        {
            StartCoroutine(SpawnWave());
            // nextWaveTime wordt ingesteld in SpawnWave na het spawnen
            currentWaveIndex++;
            Debug.Log("Start nieuwe golf: " + currentWaveIndex);

            if (currentWaveIndex % 3 == 0 && activeEnemyTypes.Count < allEnemyTypes.Count)
            {
                activeEnemyTypes.Add(allEnemyTypes[activeEnemyTypes.Count]);
                Debug.Log("Nieuw enemy type toegevoegd. Totaal: " + activeEnemyTypes.Count);
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        if (spawnPoint == null || activeEnemyTypes.Count == 0 || waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("SpawnWave mislukt: spawnPoint, enemyTypes, of waypoints ongeldig.");
            yield break;
        }

        foreach (GameObject enemyType in activeEnemyTypes)
        {
            for (int i = 0; i < enemiesPerTypePerWave; i++) // Vaste aantal per type
            {
                GameObject newEnemy = Instantiate(enemyType);
                if (newEnemy == null)
                {
                    Debug.LogError("Instantiatie van prefab mislukt voor: " + enemyType.name);
                    continue;
                }
                newEnemy.transform.position = spawnPoint.position;
                newEnemy.SetActive(true);
                BaseEnemy enemyScript = newEnemy.GetComponent<BaseEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.SetWaypoints(waypoints); // Geef Transform[] waypoints door
                    Debug.Log("Vijand gespawnd: " + enemyType.name + " (index " + i + ")");
                }
                else
                {
                    Debug.LogWarning("Geen BaseEnemy-component op vijand: " + enemyType.name);
                }
                activeEnemies.Add(newEnemy);
                yield return new WaitForSeconds(spawnDelay); // Wacht tussen elke vijand
            }
        }

        // Stel nextWaveTime in na het spawnen van alle vijanden
        nextWaveTime = Time.time + waveInterval;
        Debug.Log("Golf " + currentWaveIndex + " spawning voltooid. Volgende golf over: " + waveInterval + " seconden.");
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count == 0)
            Debug.Log("[WaveManager] Golf " + currentWaveIndex + " voltooid!");
    }
}