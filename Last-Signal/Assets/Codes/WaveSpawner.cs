//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WaveSpawner : MonoBehaviour
//{
//    // Struct om een wave te definiëren, configureerbaar in de Unity Inspector
//    [System.Serializable]
//    public class Wave
//    {
//        // Array van enemy prefabs die in deze wave spawnen
//        public GameObject[] enemyPrefabs;

//        // Aantal van elke enemy-type dat gespawnd wordt
//        public int[] enemyCounts;

//        // Vertraging tussen het spawnen van individuele enemies (in seconden)
//        public float spawnDelay;

//        // Vertraging voordat de volgende wave begint (in seconden)
//        public float waveDelay;
//    }

//    // Array van waves, configureerbaar in de Inspector
//    public Wave[] waves;

//    // Array van spawnpunten waar enemies kunnen verschijnen
//    public Transform[] spawnPoints;

//    // Array van waypoints die het pad definiëren voor alle enemies
//    public Transform[] waypoints;

//    // Huidige wave-index
//    private int currentWaveIndex = 0;

//    // Vlag om te controleren of een wave momenteel wordt gespawnd
//    private bool isSpawning = false;

//    // Start functie, aangeroepen bij initialisatie van het script
//    void Start()
//    {
//        // Controleer of de benodigde arrays zijn ingesteld
//        if (waves.Length == 0)
//        {
//            Debug.LogError("Geen waves ingesteld in WaveSpawner!");
//            return;
//        }
//        if (spawnPoints.Length == 0)
//        {
//            Debug.LogError("Geen spawnpunten ingesteld in WaveSpawner!");
//            return;
//        }
//        if (waypoints.Length == 0)
//        {
//            Debug.LogError("Geen waypoints ingesteld in WaveSpawner!");
//            return;
//        }

//        // Start de eerste wave na de ingestelde vertraging
//        StartCoroutine(StartWaveAfterDelay(waves[0].waveDelay));
//    }

//    // Coroutine om een wave te starten na een vertraging
//    IEnumerator StartWaveAfterDelay(float delay)
//    {
//        // Wacht de opgegeven tijd voordat de wave begint
//        Debug.Log("Wachten " + delay + " seconden voordat wave " + (currentWaveIndex + 1) + " begint.");
//        yield return new WaitForSeconds(delay);

//        // Start de wave
//        StartCoroutine(SpawnWave());
//    }

//    // Coroutine om een wave van enemies te spawnen
//    IEnumerator SpawnWave()
//    {
//        // Stop als we al aan het spawnen zijn of als er geen waves meer zijn
//        if (isSpawning || currentWaveIndex >= waves.Length)
//        {
//            Debug.Log("Geen waves meer of al aan het spawnen!");
//            yield break;
//        }

//        isSpawning = true; // Markeer dat we aan het spawnen zijn
//        Wave currentWave = waves[currentWaveIndex]; // Huidige wave

//        Debug.Log("Start wave " + (currentWaveIndex + 1));

//        // Loop door alle enemy-types in de wave
//        for (int i = 0; i < currentWave.enemyPrefabs.Length; i++)
//        {
//            // Controleer of de prefab en het aantal geldig zijn
//            if (currentWave.enemyPrefabs[i] == null || currentWave.enemyCounts[i] <= 0)
//            {
//                Debug.LogWarning("Ongeldige enemy-prefab of aantal in wave " + (currentWaveIndex + 1));
//                continue;
//            }

//            // Spawn het gespecificeerde aantal enemies van dit type
//            for (int j = 0; j < currentWave.enemyCounts[i]; j++)
//            {
//                // Kies een willekeurig spawnpunt
//                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

//                // Instantieer de enemy op het spawnpunt
//                GameObject enemy = Instantiate(currentWave.enemyPrefabs[i], spawnPoint.position, spawnPoint.rotation);
//                Debug.Log("Spawned " + enemy.name + " op " + spawnPoint.position);

//                // Stel het pad in voor de enemy
//                BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
//                if (enemyScript != null)
//                {
//                    enemyScript.waypoints = waypoints; // Wijs het pad toe
//                    Debug.Log("Waypoints toegewezen aan " + enemy.name);
//                }
//                else
//                {
//                    Debug.LogError("Enemy " + enemy.name + " heeft geen BaseEnemy-component!");
//                }

//                // Wacht de spawn-vertraging voordat de volgende enemy wordt gespawnd
//                yield return new WaitForSeconds(currentWave.spawnDelay);
//            }
//        }

//        // Wave is klaar
//        isSpawning = false;
//        currentWaveIndex++; // Ga naar de volgende wave
//        Debug.Log("Wave " + currentWaveIndex + " voltooid.");

//        // Controleer of er meer waves zijn
//        if (currentWaveIndex < waves.Length)
//        {
//            // Start de volgende wave na de wave-vertraging
//            StartCoroutine(StartWaveAfterDelay(waves[currentWaveIndex].waveDelay));
//        }
//        else
//        {
//            Debug.Log("Alle waves zijn voltooid!");
//        }
//    }
//}
