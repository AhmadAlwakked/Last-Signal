using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private float waveInterval = 30f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<WaveConfig> waveConfigs = new List<WaveConfig>();
    private int currentWaveIndex = 0;
    private float nextWaveTime;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private Dictionary<GameObject, Queue<GameObject>> enemyPools = new Dictionary<GameObject, Queue<GameObject>>();
    [SerializeField] private int maxPoolSizePerEnemy = 10;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        InitializePools();
    }

    private void Start() => nextWaveTime = Time.time + waveInterval;

    private void Update()
    {
        if (Time.time >= nextWaveTime && currentWaveIndex < waveConfigs.Count)
        {
            StartCoroutine(SpawnWave());
            nextWaveTime = Time.time + waveInterval;
        }
    }

    private void InitializePools()
    {
        foreach (var wave in waveConfigs)
        {
            foreach (var enemyType in wave.enemyTypes)
            {
                if (!enemyPools.ContainsKey(enemyType.enemyPrefab))
                {
                    enemyPools[enemyType.enemyPrefab] = new Queue<GameObject>();
                    for (int i = 0; i < maxPoolSizePerEnemy; i++)
                    {
                        GameObject enemy = Instantiate(enemyType.enemyPrefab);
                        enemy.SetActive(false);
                        enemyPools[enemyType.enemyPrefab].Enqueue(enemy);
                    }
                }
            }
        }
    }

    private GameObject GetPooledEnemy(GameObject prefab)
    {
        if (enemyPools.ContainsKey(prefab) && enemyPools[prefab].Count > 0)
        {
            return enemyPools[prefab].Dequeue();
        }
        return Instantiate(prefab);
    }

    private void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        BaseClass enemyScript = enemy.GetComponent<BaseClass>();
        if (enemyScript != null && enemyScript.CurrentHP <= 0)
        {
            GameObject prefab = enemyScript.GetComponent<BaseClass>().gameObject;
            if (prefab && enemyPools.ContainsKey(prefab))
            {
                enemyPools[prefab].Enqueue(enemy);
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex >= waveConfigs.Count) yield break;
        WaveConfig wave = waveConfigs[currentWaveIndex++];
        foreach (var enemyType in wave.enemyTypes)
        {
            int count = Random.Range(enemyType.minCount, enemyType.maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = GetPooledEnemy(enemyType.enemyPrefab);
                enemy.transform.position = spawn.position;
                enemy.SetActive(true);
                activeEnemies.Add(enemy);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        ReturnEnemyToPool(enemy);
        if (activeEnemies.Count == 0 && currentWaveIndex >= waveConfigs.Count)
            Debug.Log("[WaveManager] Alle golven voltooid!");
    }
}

[CreateAssetMenu(fileName = "WaveConfig", menuName = "TowerDefense/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] public int startWave;
    [SerializeField] public List<EnemyTypeConfig> enemyTypes = new List<EnemyTypeConfig>();
}

[System.Serializable]
public class EnemyTypeConfig
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public int minCount;
    [SerializeField] public int maxCount;
}