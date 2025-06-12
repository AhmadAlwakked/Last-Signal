using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;

    public Wave[] waves;
    private int currentWaveIndex = 0;

    private void Update()   
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
           StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()

    {
        for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
        {
            Instantiate(waves[currentWaveIndex].enemies[i], spawnPoint.transform);
            yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
        }
    }

    [System.Serializable]
    public class Wave
    {
        public Enemy[] enemies;
        public float timeToNextEnemy;

    }













}
    