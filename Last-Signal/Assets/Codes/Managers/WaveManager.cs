using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private float waveInterval = 30f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private EnemyTypeConfig[] enemyTypes; // Array van vijandconfiguraties
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float spawnDelay = 0.5f;

    private float nextWaveTime;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaveIndex = 0;
    private bool isSpawningWave = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Debug.Log("WaveManager geactiveerd. Aantal enemy types: " + (enemyTypes != null ? enemyTypes.Length : 0));
        if (spawnPoint == null) Debug.LogError("Geen spawnPoint ingesteld!");
        if (waypoints == null || waypoints.Length == 0) Debug.LogWarning("Geen waypoints ingesteld!");
    }

    private void Start()
    {
        nextWaveTime = Time.time + waveInterval;
        Debug.Log("Start met enemy types: " + (enemyTypes != null ? enemyTypes.Length : 0));
        if (enemyTypes != null && enemyTypes.Length >= 2)
        {
            Debug.Log("Actieve enemy types ingesteld.");
        }
        else
        {
            Debug.LogWarning("Te weinig enemy types! Minimaal 2 vereist.");
        }
    }

    private void Update()
    {
        if (Time.time >= nextWaveTime && !isSpawningWave && spawnPoint != null)
        {
            isSpawningWave = true;
            StartCoroutine(SpawnWave());
            currentWaveIndex++;
            Debug.Log("Start nieuwe golf: " + currentWaveIndex);
        }
    }

    private IEnumerator SpawnWave()
    {
        if (spawnPoint == null || enemyTypes == null || enemyTypes.Length == 0 || waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("SpawnWave mislukt: spawnPoint, enemyTypes, of waypoints ongeldig.");
            isSpawningWave = false;
            yield break;
        }

        foreach (EnemyTypeConfig enemyConfig in enemyTypes)
        {
            int waveOffset = Mathf.Max(0, currentWaveIndex - enemyConfig.startWave + 1);
            int totalEnemies = (waveOffset == 1) ? enemyConfig.baseAmount : enemyConfig.baseAmount + (waveOffset - 1) * enemyConfig.increasePerWave;
            Debug.Log($"Golf {currentWaveIndex}: {totalEnemies} vijanden van {enemyConfig.enemyPrefab.name}");

            for (int i = 0; i < totalEnemies; i++)
            {
                if (enemyConfig.enemyPrefab != null)
                {
                    GameObject newEnemy = Instantiate(enemyConfig.enemyPrefab, spawnPoint.position, Quaternion.identity);
                    if (newEnemy != null)
                    {
                        BaseEnemy enemyScript = newEnemy.GetComponent<BaseEnemy>();
                        if (enemyScript != null)
                        {
                            enemyScript.SetWaypoints(waypoints);
                            activeEnemies.Add(newEnemy);
                            Debug.Log($"Vijand gespawnd: {enemyConfig.enemyPrefab.name} (index {i})");
                        }
                        else
                        {
                            Debug.LogWarning($"Geen BaseEnemy-component op {enemyConfig.enemyPrefab.name}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Instantiatie mislukt voor {enemyConfig.enemyPrefab.name}");
                    }
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
        }

        nextWaveTime = Time.time + waveInterval; // Countdown begint na spawning
        isSpawningWave = false;
        Debug.Log($"Golf {currentWaveIndex} spawning voltooid. Volgende golf over: {waveInterval} seconden.");
    }

    public void RemoveEnemy(GameObject enemy)
    {
        BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
        if (enemyScript != null)
        {
            activeEnemies.Remove(enemy);
            if (activeEnemies.Count == 0)
            {
                Debug.Log($"Golf {currentWaveIndex} voltooid!");
            }
        }
    }

    public void SkipWave()
    {
        if (isSpawningWave) StopAllCoroutines();
        nextWaveTime = Time.time;
        currentWaveIndex++;
        StartCoroutine(SpawnWave());
        Debug.Log($"Wave {currentWaveIndex} geskipt en direct gestart.");
    }
}

[System.Serializable]
public class EnemyTypeConfig
{
    public GameObject enemyPrefab; // Prefab uit Assets
    public int baseAmount = 3; // Aantal vijanden in eerste golf
    public int startWave = 1; // Vanaf welke golf dit type spawnt
    public int increasePerWave = 1; // Toename per golf na de eerste
}