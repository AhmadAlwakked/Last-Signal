using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // De vijand die je wilt spawnen
    public float spawnInterval = 3f; // Elke 3 seconden een vijand

    private float timer = 0f;
    private List<Vector3> waypoints = new List<Vector3>(); // Lijst met waypoints van de route

    void Start()
    {
        // Definieer de 22 waypoints (voorbeeldcoördinaten, pas deze aan naar jouw route)
        waypoints.Add(new Vector3(-122, 1, 0));    // Waypoint 1
        waypoints.Add(new Vector3(10, 1, 0));   // Waypoint 2
        waypoints.Add(new Vector3(-412, 1, 20));  // Waypoint 3
        waypoints.Add(new Vector3(-415, 1, 234));  // Waypoint 4
        waypoints.Add(new Vector3(30, 1, 0));   // Waypoint 5
        waypoints.Add(new Vector3(40, 1, 0));   // Waypoint 6
        waypoints.Add(new Vector3(40, 1, -10)); // Waypoint 7
        waypoints.Add(new Vector3(50, 1, -10)); // Waypoint 8
        waypoints.Add(new Vector3(50, 1, 10));  // Waypoint 9
        waypoints.Add(new Vector3(60, 1, 10));  // Waypoint 10
        waypoints.Add(new Vector3(60, 1, 20));  // Waypoint 11
        waypoints.Add(new Vector3(70, 1, 20));  // Waypoint 12
        waypoints.Add(new Vector3(70, 1, 0));   // Waypoint 13
        waypoints.Add(new Vector3(80, 1, 0));   // Waypoint 14
        waypoints.Add(new Vector3(80, 1, -20)); // Waypoint 15
        waypoints.Add(new Vector3(90, 1, -20)); // Waypoint 16
        waypoints.Add(new Vector3(90, 1, 0));   // Waypoint 17
        waypoints.Add(new Vector3(100, 1, 0));  // Waypoint 18
        waypoints.Add(new Vector3(100, 1, 10)); // Waypoint 19
        waypoints.Add(new Vector3(110, 1, 10)); // Waypoint 20
        waypoints.Add(new Vector3(110, 1, -10)); // Waypoint 21
        waypoints.Add(new Vector3(120, 1, -10)); // Waypoint 22
    }

    void Update()
    {
        // Timer om vijanden te spawnen
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && waypoints.Count > 0)
        {
            GameObject enemy = Instantiate(enemyPrefab, waypoints[0], Quaternion.identity);
            EnemyPathfinding pathfinding = enemy.AddComponent<EnemyPathfinding>();
            if (pathfinding != null)
            {
                pathfinding.SetWaypoints(new List<Vector3>(waypoints)); // Kopieer waypoints
            }
        }
    }
}

public class EnemyPathfinding : MonoBehaviour
{
    private List<Vector3> waypoints;
    private int currentWaypointIndex = 0;
    public float speed = 5f; // Snelheid van de vijand
    public float rotationSpeed = 5f; // Snelheid van rotatie (optioneel)

    public void SetWaypoints(List<Vector3> newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0; // Reset index bij nieuwe waypoints
    }

    void Update()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Count) return;

        // Beweeg naar het huidige waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Laat de vijand naar het waypoint kijken
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Als de vijand dichtbij het waypoint is, ga naar het volgende waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                Destroy(gameObject); // Verwijder de vijand aan het einde van de route
            }
        }
    }
}