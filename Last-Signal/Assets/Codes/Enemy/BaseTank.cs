using UnityEngine;

public class BaseTank : BaseEnemy
{
    [SerializeField, Tooltip("Turret child object")]
    private TankTurret turret; // Referentie naar de turret
    [SerializeField, Tooltip("Afstand om aan de kant te gaan")]
    private float sideMoveDistance = 2f; // Zijwaartse beweging
    [SerializeField, Tooltip("Snelheid van zijwaartse beweging")]
    private float sideMoveSpeed = 2f;
    [SerializeField, Tooltip("Stopafstand vanaf laatste waypoint")]
    private float stopDistance = 0.5f; // Afstand om te stoppen

    private bool hasMovedAside = false; // Eenvoudige vlag voor zijwaartse beweging
    private Vector3 targetAsidePosition; // Doelpositie voor zijwaartse beweging

    protected override void Start()
    {
        base.Start();
        if (turret == null)
        {
            turret = GetComponentInChildren<TankTurret>();
            if (turret == null)
            {
                Debug.LogError("No TankTurret found on children!");
            }
        }
    }

    protected override void UpdateLogic()
    {
        if (!isAlive) return;

        if (isMoving)
        {
            base.UpdateLogic(); // Beweeg naar waypoints
            if (currentWaypointIndex >= waypoints.Length)
            {
                Vector3 lastWaypoint = waypoints[waypoints.Length - 1].position;
                if (Vector3.Distance(transform.position, lastWaypoint) <= stopDistance)
                {
                    isMoving = false;
                }
            }
        }
        else if (!hasMovedAside)
        {
            StartMovingAside();
            hasMovedAside = true;
        }
        else if (hasMovedAside)
        {
            MoveAside();
        }
    }

    private void StartMovingAside()
    {
        Debug.Log("Tank begint aan de kant te bewegen.");
        targetAsidePosition = transform.position + transform.right * sideMoveDistance;
    }

    private void MoveAside()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetAsidePosition, sideMoveSpeed * Time.deltaTime);

    }

    protected override void Die()
    {
        base.Die();
        if (turret != null)
        {
            turret.gameObject.SetActive(false); // Deactiveer turret bij dood
        }
    }
}