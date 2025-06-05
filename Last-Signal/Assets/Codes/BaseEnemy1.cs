using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy1 : BaseClass
{
    // Array van waypoints die het pad definiëren dat de enemy volgt
    // Wordt ingesteld via de Unity Inspector of via code
    public Transform[] waypoints;

    // Huidige index van het waypoint waar de enemy naartoe beweegt
    protected int currentWaypointIndex = 0;

    // Snelheid waarmee de enemy beweegt, configureerbaar in de Inspector
    public float moveSpeed = 5f;

    // Vlag om te controleren of de enemy het eindpunt heeft bereikt
    protected bool hasReachedEnd = false;

    // Virtuele Start functie, aangeroepen bij initialisatie van de enemy
    protected virtual void Start()
    {
        // Controleer of waypoints zijn ingesteld
        // Dit voorkomt fouten als het pad niet is geconfigureerd
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Geen waypoints ingesteld voor " + gameObject.name + "! Enemy wordt uitgeschakeld.");
            enabled = false; // Schakel het script uit om fouten te voorkomen
            return;
        }
    }

    // Virtuele Update functie, aangeroepen elke frame
    protected virtual void Update()
    {
        // Stop met updaten als de enemy het eindpunt heeft bereikt
        if (hasReachedEnd) return;

        // Beweeg naar het huidige waypoint
        MoveToWaypoint();

        // Controleer of de enemy alle waypoints heeft bereikt
        if (currentWaypointIndex >= waypoints.Length)
        {
            OnReachEnd(); // Roep de functie aan voor eindpunt-gedrag
        }
    }

    // Virtuele functie voor beweging naar een waypoint
    // Kan overschreven worden in afgeleide klassen voor aangepaste beweging (bijv. vliegende enemies)
    protected virtual void MoveToWaypoint()
    {
        // Controleer of er nog waypoints zijn om naartoe te bewegen
        if (currentWaypointIndex >= waypoints.Length) return;

        // Haal de positie van het huidige waypoint op
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;

        // Bereken de richting naar het waypoint
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Beweeg de enemy naar het waypoint met de ingestelde snelheid
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Controleer of de enemy dicht genoeg bij het waypoint is (drempel van 0.1 eenheden)
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Ga naar het volgende waypoint
            currentWaypointIndex++;
        }
    }

    // Virtuele functie die wordt aangeroepen als de enemy het eindpunt bereikt
    // Standaard gedrag: enemy stopt met bewegen en blijft leven
    protected virtual void OnReachEnd()
    {
        hasReachedEnd = true; // Markeer dat de enemy het eindpunt heeft bereikt
        Debug.Log(gameObject.name + " heeft het eindpunt bereikt en blijft leven.");
        // Afgeleide klassen kunnen dit overschrijven voor ander gedrag (bijv. exploderen)
    }

    // Overschrijf de TakeDamage functie van BaseClass
    // Wordt aangeroepen wanneer de enemy schade ontvangt
    public override void TakeDamage(float damage)
    {
        // Roep de basisfunctie aan om HP te verminderen
        base.TakeDamage(damage);

        // Controleer of de enemy dood is
        if (currentHealth <= 0)
        {
            OnDeath(); // Roep de virtuele functie aan voor dood-gedrag
        }
    }

    // Virtuele functie voor als de enemy doodgaat
    // Standaard gedrag: vernietig het GameObject
    protected virtual void OnDeath()
    {
        Debug.Log(gameObject.name + " is vernietigd.");
        Destroy(gameObject); // Verwijder de enemy uit de scene
    }
}
