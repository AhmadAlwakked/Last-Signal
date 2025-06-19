using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private float waveInterval = 30f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> allEnemyTypes = new List<GameObject>();
    [SerializeField] private int maxPoolSizePerEnemy = 10;
    [SerializeField] private int baseEnemiesPerType = 1;
    [SerializeField] private int enemyIncreasePerWave = 2;
    [SerializeField] private float spawnDelay = 1f; // Nieuwe aanpasbare delay tussen spawns

    private Dictionary<GameObject, Queue<GameObject>> enemyPools = new Dictionary<GameObject, Queue<GameObject>>();
    private float nextWaveTime;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaveIndex = 0;
    private List<GameObject> activeEnemyTypes = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        InitializePools();
    }

    private void Start()
    {
        nextWaveTime = Time.time + waveInterval;
        if (allEnemyTypes.Count >= 2)
        {
            activeEnemyTypes.Add(allEnemyTypes[0]);
            activeEnemyTypes.Add(allEnemyTypes[1]);
        }
    }

    private void Update()
    {
        if (Time.time >= nextWaveTime && spawnPoint != null && activeEnemyTypes.Count > 0)
        {
            StartCoroutine(SpawnWave());
            nextWaveTime = Time.time + waveInterval;
            currentWaveIndex++;

            if (currentWaveIndex % 3 == 0 && activeEnemyTypes.Count < allEnemyTypes.Count)
            {
                activeEnemyTypes.Add(allEnemyTypes[activeEnemyTypes.Count]);
            }
        }
    }

    private void InitializePools()
    {
        foreach (GameObject enemyPrefab in allEnemyTypes)
        {
            enemyPools[enemyPrefab] = new Queue<GameObject>();
            for (int i = 0; i < maxPoolSizePerEnemy; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.SetActive(false);
                enemyPools[enemyPrefab].Enqueue(enemy);
            }
        }
    }

    private GameObject GetPooledEnemy(GameObject prefab)
    {
        if (enemyPools.ContainsKey(prefab) && enemyPools[prefab].Count > 0)
        {
            return enemyPools[prefab].Dequeue();
        }
        GameObject enemy = Instantiate(prefab);
        enemy.SetActive(false);
        return enemy;
    }

    private void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        BaseClass enemyScript = enemy.GetComponent<BaseClass>();
        if (enemyScript != null && enemyScript.CurrentHP <= 0)
        {
            GameObject prefab = enemyScript.gameObject;
            if (prefab && enemyPools.ContainsKey(prefab))
            {
                enemyPools[prefab].Enqueue(enemy);
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        if (spawnPoint == null || activeEnemyTypes.Count == 0) yield break;

        foreach (GameObject enemyType in activeEnemyTypes)        {
            int totalEnemies = baseEnemiesPerType + (currentWaveIndex * enemyIncreasePerWave);
            for (int i = 0; i < totalEnemies; i++)
            {
                GameObject enemy = GetPooledEnemy(enemyType);
                enemy.transform.position = spawnPoint.position;
                enemy.SetActive(true);
                activeEnemies.Add(enemy);
                yield return new WaitForSeconds(spawnDelay); // Gebruik de aanpasbare delay
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        ReturnEnemyToPool(enemy);
        if (activeEnemies.Count == 0)
            Debug.Log("[WaveManager] Golf " + currentWaveIndex + " voltooid!");
    }
}